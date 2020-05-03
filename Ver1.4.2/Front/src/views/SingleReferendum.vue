
<template>
  <div>  

    <h2>{{ $activeUserName }}, welcome to referendum! </h2>
    <!--<button v-on:click="GetJsonSingleProposition" class="refresh">Refresh</button><br>-->
    <ul>{{propositions[0].proposition}}
        <li v-for="element in propositions">
            <span>{{element.appellation}}</span>
            <span class = "blockButtonVote">
                <span>{{element.amount}}</span>
                <button v-on:click="SetVote(element.idAnswer)" v-if="VoteVisible()">Vote</button>           
            </span>
        </li>
    </ul> 
    <button v-on:click="visibleAddAnswer=!visibleAddAnswer" v-if="$activeUserName != 'Guest'" class="main">Add answer</button>
    <div v-if="visibleAddAnswer" class="addReferendum">
        <label>Enter your answer</label>
        <input type="text" v-model="newAnswer"/><br>
        <span>Your answer: {{newAnswer}}</span><br>
        <button v-on:click="SendAnswer">Save answer</button>
    </div>

  </div>
</template>

<script>
export default {
    props: ['id'],
    data () {
        return {
            propositions: [{proposition: ''}],
            votesAmount: 100,
            visibleAddAnswer: false,
            newAnswer: ''
        }
    },    

    methods: {

        GetJsonSingleProposition: async function() {                                
            let url = 'https://localhost:44372/api/Home/' + this.id + '/GetSingleReferendum';
            let user = {                
                    id: Number(this.$activeUserId)                    
                };
            let response = await fetch(url, {
                method: 'POST',                    
                headers: {
                    'Content-Type': 'application/json;charset=utf-8'                                              
                    },
                body: JSON.stringify(user)                   
                });
            let responseJson = await response.json();
            this.propositions = [];
                responseJson.forEach(element => {                    
                    this.propositions.push(element);
                    //console.log(element)                   
                });
            this.propositions.forEach(element => {
                    if (element.votesAmount < this.votesAmount) this.votesAmount = element.votesAmount;
                });

        },

        VoteVisible: function() {
                //console.log(item.deadLine);
                //console.log(new Date());
                if (this.$activeUserName != 'Guest' && this.votesAmount > 0) return true;
                else return false;                
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
                this.GetJsonSingleProposition();                
            },

        SendAnswer: async function() {
                let url = 'https://localhost:44372/api/Home/5/SendAnswer';
                let answer = {
                    Referendum: this.propositions[0].id,
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
                this.GetJsonSingleProposition();   
            }

    },

    mounted() {
            this.GetJsonSingleProposition();
        }

}

</script>

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
        margin: 0px 0px;
    }

</style>
