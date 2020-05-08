<template>
  <div class="hello">    
    <h2>{{ $activeUserName }}, welcome to referendums! </h2>    
    <button v-on:click="visibleAddReferendum=!visibleAddReferendum" v-if="$activeUserName != 'Guest'" class="main">Add referendum</button> 
    <button v-on:click="GetMyReferendums" v-if="$activeUserName != 'Guest'" class="main">{{buttonMyRefer}}</button> 
    <div v-if="visibleAddReferendum" class="addReferendum">        
        <label>Proposition</label>
        <input type="text" v-model="proposition" placeholder="Enter proposition"/><br>
        <label>Maximum personal responses</label>
        <input type="number" min="0" v-model="maxOwnAnswers" placeholder="Select" class="number"/><br>
        <label>Maximum answers</label>
        <input type="number" min="1" v-model="maxAmountAnswers" placeholder="Select" class="number"/><br>
        <label>Dead line</label>
        <input type="text" v-model="deadLine" placeholder="Example: 2020-12-31"/><br>
        <!--<label>Publish date</label>
        <input type="text" v-model="publishDate" placeholder="Example: 2020-04-17"/><br>-->
        <button v-on:click="SendReferendum" class="saveRefer">Save referendum</button>         
    </div>  
            
    <div v-for="item in namesProposition" class="proposition" v-if="Visible(item)">
        <div>
            <h2>
                {{item.proposition}} 
            </h2>            
            <span>
                Autor: {{item.autor}} 
            </span> <br>
            <span>
                Publish date: {{item.publishDate}} 
            </span> <br>
            <a v-bind:href=item.urlHreference>http://localhost:8080/SingleReferendum/{{item.id}}</a>  
        </div>      
    </div>

    <div v-for="item in myNamesProposition" v-if="visibleMyReferendums && isPropositionPublic" class="proposition">
        <h2>
            {{item}}              
        </h2> 
        <div v-for="elem in myNamesAppellation" v-if="elem.proposition == item">
            {{elem.appellation}} : 
            <span v-for="element in myPropositions" v-if="element.proposition == item && element.appellation == elem.appellation && element.answerAutor != 'TestUser'">
                {{element.answerAutor}}, 
            </span>   
        </div> 
    </div> 

    <!--<div v-for="item in myPropositionsNotPublic" v-if="visibleMyReferendums && isPropositionPublic" class="proposition">
        <h2>
            {{item.proposition}} 
            <button v-on:click="visibleAddAnswer=!visibleAddAnswer" class="public">Add answer</button> 
            <button v-on:click="Publish(item.id)" v-if="isAnswer" class="public">Public</button> 
        </h2> 
        <div v-if="visibleAddAnswer" >
            <label>Enter your answer: </label>
            <input type="text" v-model="newAnswer"/><br>
            <span>Your answer: {{newAnswer}}</span><br>
            <button v-on:click="SendAnswer(item.id)">Save answer</button>
        </div>
    </div> --> 

    <div v-for="item in myPropositionsNotPublic" v-if="visibleMyReferendums" class="proposition">
        <MyEditableReferendum v-bind:Referendum=item  @refresh="RefreshMyReferendums" />
    </div>    

  </div>
</template>

<script>
import MyEditableReferendum from './MyEditableReferendum.vue'

export default { 
    name: 'Referendum',
    components: {
    MyEditableReferendum
  },  
  data () {
    return {      
            answer: '',           
            propositions: [],
            namesProposition : [],           
            proposition: '',
            maxOwnAnswers: '',
            maxAmountAnswers: '',
            deadLine: '2020-12-31',
            publishDate: '2020-04-17',
            visibleAddReferendum: false,
            visibleAllReferendums: true,
            myPropositions: [],
            myNamesProposition: [],
            myNamesAppellation: [],
            visibleMyReferendums: false,
            buttonMyRefer: 'My referendums', 
            myPropositionsNotPublic: [],
            isPropositionPublic: false,
            //newAnswer: '',
            //visibleAddAnswer: false,
            //isAnswer: false,

    }
  },
    methods:{        

            GetJsonPropositions: async function(){                                
                let url = 'https://localhost:44372/api/Home/5/GetReferendums';
               
                let response = await fetch(url, {
                    method: 'GET',                    
                    headers: {
                    'Content-Type': 'application/json;charset=utf-8'                                             
                    },                                   
                });          

                let responseJson = await response.json();
                this.propositions = [];
                responseJson.forEach(element => {                    
                    this.propositions.push(element)                   
                });
                this.namesProposition = [];               
                this.propositions.forEach(item => {

                    this.namesProposition.push({
                            proposition: item.proposition,                            
                            id: item.id,
                            urlHreference: "SingleReferendum/" + item.id,
                            autor: item.autor,
                            publishDate: item.publishDate,
                            published: item.published
                        });
                });                
            },

            RefreshMyReferendums: function() {
                this.myPropositionsNotPublic = [];
                this.GetMyReferendums();
                this.GetMyReferendums();
            },

            GetMyReferendums: async function() {
                if (this.buttonMyRefer == 'My referendums') {
                    this.buttonMyRefer = 'All Referendums';
                    }
                else this.buttonMyRefer = 'My referendums';
                this.visibleAllReferendums = !this.visibleAllReferendums;
                this.visibleMyReferendums = !this.visibleMyReferendums;
                let url = 'https://localhost:44372/api/Home/' + this.$activeUserId + '/GetMyReferendums';
                let response = await fetch(url, {
                    method: 'GET',                    
                    headers: {
                    'Content-Type': 'application/json;charset=utf-8'                                             
                    },                                   
                });
                let responseJson = await response.json();
                this.myPropositions = [];
                responseJson.forEach(element => {                    
                    if (element.published == 1) this.myPropositions.push(element)                   
                });
                this.myNamesProposition = [];
                
                if (this.myPropositions[0] != undefined) {
                    this.isPropositionPublic = true;
                    //console.log(this.isPropositionPublic);
                    this.myNamesProposition.push(this.myPropositions[0].proposition);               
                    this.myPropositions.forEach(item => {
                        let isInArray = false;
                        for (let i = 0; i < this.myNamesProposition.length; i++) {
                            if (this.myNamesProposition[i] == item.proposition) {                            
                                isInArray = true;                            
                                break;
                            }                        
                        }
                        if (! isInArray) {                        
                            this.myNamesProposition.push(item.proposition)
                        }
                    });
                    this.myNamesAppellation = [];
                    this.myNamesAppellation.push({
                        appellation: this.myPropositions[0].appellation,
                        proposition: this.myPropositions[0].proposition
                        });
                    this.myPropositions.forEach(item => {
                        let isInArray = false;
                        for (let i = 0; i < this.myNamesAppellation.length; i++) {
                            if (this.myNamesAppellation[i].appellation == item.appellation) {                            
                                isInArray = true;                            
                                break;
                            }                        
                        }
                        if (! isInArray) {                        
                            this.myNamesAppellation.push({
                                appellation: item.appellation,
                                proposition: item.proposition
                                })
                        }
                    });
                }
                this.GetMyReferendumsNotPublic();

            },

            GetMyReferendumsNotPublic: async function() {
                let urlNotPublic = 'https://localhost:44372/api/Home/' + this.$activeUserId + '/GetMyReferendumsNotPublic';
                let responseNotPublic = await fetch(urlNotPublic, {
                    method: 'GET',                    
                    headers: {
                    'Content-Type': 'application/json;charset=utf-8'                                             
                    },                                   
                });
                let responseJsonNotPublic = await responseNotPublic.json();
                this.myPropositionsNotPublic = [];
                responseJsonNotPublic.forEach(element => {                    
                    this.myPropositionsNotPublic.push(element)                   
                });                   
            },

            /*SendAnswer: async function(idReferendum) {
                //console.log(id);
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
            },

            Publish: async function(idReferendum) {
                let url = 'https://localhost:44372/api/Home/' + idReferendum + '/Publish';
                let response = await fetch(url, {
                    method: 'GET',                    
                    headers: {
                    'Content-Type': 'application/json;charset=utf-8'                                             
                    },                                   
                });
            },*/

            Visible: function(item) {               
                if (this.visibleAllReferendums && item.published != 0) return true;
                else return false;                
            },
             
            SendReferendum: async function(nameUser) {
                let url = 'https://localhost:44372/api/Home/5/AddReferendum';
                let referendum = {
                    Proposition: this.proposition,
                    Autor: this.$activeUserId, 
                    Active: 1,
                    Published: 0,
                    MaxOwnAnswers: Number(this.maxOwnAnswers),
                    MaxAmountAnswers: Number(this.maxAmountAnswers),
                    DeadLine: this.deadLine,
                    PublishDate: this.publishDate
                };
                let response = await fetch(url, {
                    method: 'POST',                    
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'                                              
                    },
                    body: JSON.stringify(referendum)                   
                });
                this.proposition = '';
                this.maxOwnAnswers = '';
                this.maxAmountAnswers = ''; 
                this.visibleAddReferendum = !this.visibleAddReferendum;
                this.RefreshMyReferendums();       
            }, 

    },
    mounted() {
            this.GetJsonPropositions();
        }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

    div.proposition {
        font: 18px sans-serif;
        border-style: solid;
		    border-width: 2px;
		    border-color: #cccccc; 
        background-color: rgb(226, 243, 250);
        padding: 10px;
        margin: 20px 0px;      
    }
    span {
        font-size: 12px;
        margin: 0;
    }   
    div.addReferendum {
        background-color: #eaf6fb; 
        margin: 7px 0px;      
    }
    button.main {
        margin: 0px 20px;
    }
    a {
        font: 12px sans-serif;    
    }
    button.public {
        margin: 0px 10px;
    }
    input.number {
    width: 70px;
    margin: 3px 2px 3px 15px;
  }
  button.saveRefer {
        margin: 10px 10px;
    }

</style>
