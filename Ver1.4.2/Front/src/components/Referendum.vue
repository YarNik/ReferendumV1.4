<template>
  <div class="hello">    
    <h2>{{ $activeUserName }}, welcome to referendums! </h2>    
    <button v-on:click="visibleAddReferendum=!visibleAddReferendum" v-if="$activeUserName != 'Guest'" class="main">Add referendum</button> 
    <button v-on:click="GetMyReferendums" v-if="$activeUserName != 'Guest'" class="main">{{buttonMyRefer}}</button> 
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

    <div v-for="item in myNamesProposition" v-if="visibleMyReferendums" class="proposition">
        <h2>
            {{item}} 
        </h2> 
        <div v-for="elem in myNamesAppellation" v-if="elem.proposition == item">
            {{elem.appellation}} : 
            <span v-for="element in myPropositions" v-if="element.proposition == item && element.appellation == elem.appellation && element.answerAutor != 'TestUser'">
                {{element.answerAutor}} 
            </span>   
        </div> 

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
            proposition: '',
            maxOwnAnswers: '',
            maxAmountAnswers: '',
            deadLine: '2020-06-01',
            publishDate: '2020-04-17',
            visibleAddReferendum: false,
            visibleAllReferendums: true,
            myPropositions: [],
            myNamesProposition: [],
            myNamesAppellation: [],
            visibleMyReferendums: false,
            buttonMyRefer: 'My referendums',           
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

            GetMyReferendums: async function(item) {
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
                    this.myPropositions.push(element)                   
                });
                this.myNamesProposition = [];
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
            },

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

</style>
