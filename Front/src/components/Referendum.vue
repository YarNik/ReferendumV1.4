<template>
  <div class="hello">    
    <h2>{{ $activeUserName }}, welcome to referendums! </h2>    
    <button v-on:click="GetJsonPropositions" class="refresh">Refresh</button><br>   
            
    <ul v-for="item in namesProposition">{{item}}
        <li v-for="element in propositions" v-if="item == element.proposition">
            <span>{{element.appellation}}</span>
            <span class = "blockButtonVote">
                <span>{{element.amount}}</span>
                <button v-on:click="SetVote(element.idAnswer)" v-if="$activeUserName != 'Guest'">Vote</button> 
            </span>
        </li>        
    </ul>    
    
    <button v-on:click="visibleAddAnswer=!visibleAddAnswer" v-if="$activeUserName != 'Guest'" class="main">Add answer</button><br> 
    <div v-if="visibleAddAnswer" class="addReferendum">
        <button v-on:click="ReceiveAllPropositions" class="main">Receive propositions</button>
        <select v-model="selectedProposition">
            <option v-for="item in namesPropositionAndId" v-bind:value="item.id">{{item.proposition}}</option>
        </select><br>        
        <label>Enter your answer</label>
        <input type="text" v-model="newAnswer"/><br>
        <span>Your answer: {{newAnswer}}</span><br>
        <button v-on:click="SendAnswer">Save answer</button>  

    </div>
    <button v-on:click="visibleAddReferendum=!visibleAddReferendum" v-if="$activeUserName != 'Guest'" class="main">Add referendum</button> 

    <div v-if="visibleAddReferendum" class="addReferendum">        
        <label>Proposition</label>
        <input type="text" v-model="proposition" placeholder="Enter proposition"/><br>
        <label>Maximum personal responses</label>
        <input type="text" v-model="maxOwnAnswers" placeholder="Enter amount"/><br>
        <label>Maximum answers</label>
        <input type="text" v-model="maxAmountAnswers" placeholder="Enter amount"/><br>
        <label>Dead line</label>
        <input type="text" v-model="deadLine" placeholder="Example: 2020-06-01"/><br>
        <label>Publish date</label>
        <input type="text" v-model="publishDate" placeholder="Example: 2020-04-17"/><br>
        <button v-on:click="SendReferendum">Save referendum</button>         
    </div>  

  </div>
</template>

<script>
export default {    
  data () {
    return {      
            answer: '',           
            propositions: [],
            namesProposition : [],
            namesPropositionAndId : [],
            //nameUserId: this.$activeUserId,
            proposition: '',
            maxOwnAnswers: '',
            maxAmountAnswers: '',
            deadLine: '2020-06-01',
            publishDate: '2020-04-17',
            visibleAddReferendum: false,
            visibleAddAnswer: false,
            selectedProposition: '',
            newAnswer: ''
    }
  },
    methods:{              
            GetJsonPropositions: async function(){  
                //console.log(this.nameUserId);
                //console.log(this.$activeUserId);                            
                let url = 'https://localhost:44372/api/Home';
                let response = await fetch(url);
                let responseJson = await response.json();
                this.propositions = [];
                responseJson.forEach(element => {                    
                    this.propositions.push(element)
                });
                this.namesProposition = [];
                this.namesProposition.push(this.propositions[0].proposition);                
                this.propositions.forEach(item => {
                    let isInArray = false;
                    for (let i = 0; i < this.namesProposition.length; i++) {
                        if (this.namesProposition[i] == item.proposition) {                            
                            isInArray = true;                            
                            break;
                        }                        
                    }
                    if (! isInArray) {
                        this.namesProposition.push(item.proposition);
                    }                    
                });                
            },

            SetVote: async function(IdAnswer){
                this.answer = IdAnswer;               
                let url = 'https://localhost:44372/api/Home/5/Vote';
                let allAnswer = {
                    IdAnswer: IdAnswer,
                    IdUser: this.$activeUserId                    
                };
                let response = await fetch(url, {
                    method: 'POST',                    
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'                                              
                    },
                    body: JSON.stringify(allAnswer)                   
                });
                //GetJsonPropositions();
            },
             
            SendReferendum: async function(nameUser) {
                let url = 'https://localhost:44372/api/Home/5/AddReferendum';
                let referendum = {
                    Proposition: this.proposition,
                    Autor: this.$activeUserId, 
                    Active: 1,
                    Published: 1,
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
            }, 

            ReceiveAllPropositions: async function() {                
                let url = 'https://localhost:44372/api/Home/5/ReceiveAllPropositions';
                let response = await fetch(url);
                let responseJson = await response.json();
                this.namesPropositionAndId = [];
                responseJson.forEach(element => {                    
                    this.namesPropositionAndId.push(element)
                });
            },

            SendAnswer: async function() {
                let url = 'https://localhost:44372/api/Home/5/SendAnswer';
                let answer = {
                    Referendum: Number(this.selectedProposition),
                    Appellation: this.newAnswer,
                    AnswerAutor: Number(this.$activeUserId)
                }
                let response = await fetch(url, {
                    method: 'POST',                    
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'                                              
                    },
                    body: JSON.stringify(answer)                   
                });   
            }, 
    }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

    ul {
        font: bold 28px sans-serif;
        border-style: solid;
		    border-width: 2px;
		    border-color: #cccccc; 
        background-color: rgb(226, 243, 250);      
    }
    li {
        clear: both;
        font: bold 18px sans-serif; 
    }
    span.blockButtonVote {
        float: right;
        margin-right: 20px;
    }
    span.blockButtonVote button {
        margin-left: 20px;
    }
    button {
        margin: 0px 5px;
    }
    button.refresh {
        margin: 7px 0px;
    }
    div.addReferendum {
        background-color: #eaf6fb; 
        margin: 7px 0px;      
    }
    button.main {
        margin: 7px 0px;
    }

</style>
