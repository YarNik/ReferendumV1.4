
<template>
  <div>  
      <span class="proposition"><b>
           {{myReferendum.proposition}}</b>                          
      </span>
        <span class="edit">
          <input type="text" v-model="editPropositionName" placeholder="Edit proposition name" class="text"/>
          <button v-on:click="ChangePropositionName" class="change">Rewrite</button>
        </span>
      <br>
      <button v-on:click="visibleAddAnswer=!visibleAddAnswer" class="public">Add answer</button>
      <button v-on:click="Publish" v-if="isAnswer" class="public">Publish this referendum</button>
      <hr>
      <div v-if="visibleAddAnswer">            
            <label>Enter your answer: </label>
            <input type="text" v-model="newAnswer"/><br>
            <span>Your answer: {{newAnswer}}</span><br>
            <button v-on:click="SendAnswer(Referendum.id)">Save answer</button>
            <hr>
      </div>    
      <div v-for="item in myReferendumAnswers">
        Answer: {{item.appellation}}
        <button v-on:click="DeleteAnswer(item.id)" class="change">Delete</button>
      </div>
      <hr>
      <span>
        Maximum number of own answers: {{myReferendum.maxOwnAnswers}} 
      </span>
        <input type="number" v-model="editMaxOwnAnswers" min="0" placeholder="Select" class="number"/>
        <button v-on:click="ChangeMaxOwnAnswers" class="change">Rewrite</button>
        <br>
      <span>
        Maximum amount of answers: {{myReferendum.maxAmountAnswers}} 
      </span>
        <input type="number" v-model="editMaxAmountAnswers" min="1" placeholder="Select" class="number"/>
        <button v-on:click="ChangeMaxAmountAnswers" class="change">Rewrite</button>
        <br>
      <span>
        Dead line: {{myReferendum.deadLine}} 
      </span>
        <input type="datetime" v-model="editDeadLine" placeholder="12/31/2020 00:00:00" class="date"/>
        <button v-on:click="ChangeDeadLine" class="change">Rewrite</button>
        <br>   

  </div>
</template>

<script>
export default {
    props: ['Referendum'],
    data () {
        return {
            newAnswer: '',
            visibleAddAnswer: false,
            isAnswer: false, 
            myPropositionsNotPublic: [],
            myReferendum: { },
            myReferendumAnswers: [],
            isAnswer: false,
            editPropositionName: '',
            editMaxOwnAnswers: '',
            editMaxAmountAnswers: '',
            editDeadLine: '',
            
        }
    },
    methods:{

      SendAnswer: async function(idReferendum) {               
                let url = 'https://localhost:44372/api/Home/5/SendAnswer';
                let answer = {
                    Referendum: idReferendum,
                    Appellation: this.newAnswer,
                    AnswerAutor: Number(this.$activeUserId)
                };                
                let response = await fetch(url, {
                    method: 'POST',                    
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'                                              
                    },
                    body: JSON.stringify(answer)                   
                });
                this.isAnswer = true;
                this.newAnswer = '';
                this.visibleAddAnswer = !this.visibleAddAnswer;
                this.GetMyEditableReferendum();                    
            },     

      GetMyEditableReferendum: async function() {
                let url = 'https://localhost:44372/api/Home/' + this.Referendum.id + '/GetMyEditableReferendum';
                let response = await fetch(url, {
                    method: 'GET',                    
                    headers: {
                    'Content-Type': 'application/json;charset=utf-8'                                             
                    },                                   
                });
                this.myReferendum = await response.json();               
                this.GetMyEditableReferendumAnswers();                  
            },

      GetMyEditableReferendumAnswers: async function() {
                let url = 'https://localhost:44372/api/Home/' + this.Referendum.id + '/GetMyEditableReferendumAnswers';
                let response = await fetch(url, {
                    method: 'GET',                    
                    headers: {
                    'Content-Type': 'application/json;charset=utf-8'                                             
                    },                                   
                });
                let responseJson = await response.json();                
                this.myReferendumAnswers = [];
                responseJson.forEach(element => {                    
                    this.myReferendumAnswers.push(element)                   
                });
                if (this.myReferendumAnswers[0] != undefined) this.isAnswer = true;                   
            },

      ChangeMaxOwnAnswers: async function() {         
          let url = 'https://localhost:44372/api/Home/5/ChangeMaxOwnAnswers';
                let newReferendum = {
                    Id: this.Referendum.id,
                    MaxOwnAnswers: Number(this.editMaxOwnAnswers)                  
                };                
                let response = await fetch(url, {
                    method: 'POST',                    
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'                                              
                    },
                    body: JSON.stringify(newReferendum)                   
                });
                this.editMaxOwnAnswers = '';
                this.GetMyEditableReferendum();
      },

      ChangeMaxAmountAnswers: async function() {
          //console.log(Number(this.myReferendum.maxAmountAnswers), this.Referendum.id);
          let url = 'https://localhost:44372/api/Home/5/ChangeMaxAmountAnswers';
                let newReferendum = {
                    Id: this.Referendum.id,
                    MaxAmountAnswers: Number(this.editMaxAmountAnswers)                  
                };                
                let response = await fetch(url, {
                    method: 'POST',                    
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'                                              
                    },
                    body: JSON.stringify(newReferendum)                   
                });
                this.editMaxAmountAnswers = '';
                this.GetMyEditableReferendum();
      },

      ChangeDeadLine: async function() {
          //console.log(this.myReferendum.deadLine, this.Referendum.id);
          let url = 'https://localhost:44372/api/Home/5/ChangeDeadLine';
                let newReferendum = {
                    Id: this.Referendum.id,
                    DeadLine: this.editDeadLine                  
                };                
                let response = await fetch(url, {
                    method: 'POST',                    
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'                                              
                    },
                    body: JSON.stringify(newReferendum)                   
                });
                this.editDeadLine = '';
                this.GetMyEditableReferendum();
      },

      ChangePropositionName: async function() {          
          let url = 'https://localhost:44372/api/Home/5/ChangePropositionName';
                let newName = {
                    Id: this.Referendum.id,
                    Proposition: this.editPropositionName                    
                };                
                let response = await fetch(url, {
                    method: 'POST',                    
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'                                              
                    },
                    body: JSON.stringify(newName)                   
                });
                this.editPropositionName = '';
                this.GetMyEditableReferendum();
      },

      DeleteAnswer: async function(id) {          
          let url = 'https://localhost:44372/api/Home/' + id + '/DeleteAnswer';
                let response = await fetch(url, {
                    method: 'GET',                    
                    headers: {
                    'Content-Type': 'application/json;charset=utf-8'                                             
                    }                                   
                });
                this.isAnswer = false;
                this.GetMyEditableReferendum();
      },

      Publish: async function() {
                //console.log(this.Referendum.id);
                let url = 'https://localhost:44372/api/Home/' + this.Referendum.id + '/Publish';
                let response = await fetch(url, {
                    method: 'GET',                    
                    headers: {
                    'Content-Type': 'application/json;charset=utf-8'                                             
                    }                                   
                });
                this.$emit('refresh');                
      }, 

    },

    mounted() {
            this.GetMyEditableReferendum();
        }
}

</script>

<style scoped>

  button.public {
        margin: 15px 10px 5px 10px;
    }
  button.change {
        margin: 0px 10px;
    }
  input.number {
    width: 70px;
    margin: 3px 2px 3px 15px;
  }
  input.date {
    width: 130px;
    margin: 3px 5px 3px 10px;
  }
  span.proposition {    
    font-size: 28px;    
  }
  input.text {      
    width: 140px;
    margin: 0px 10px 0px 50px;
  }
  span.edit {
    float: right;
    white-space: nowrap;  
  }

</style>
