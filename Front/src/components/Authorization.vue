<template>
  <div>    
    <h2>{{ $activeUserName }}, welcome to referendums! </h2> 
    <p> 
        <span>Enter user name: </span>
        <input type="text" v-on:input="SetUserName" />
    </p>
    <p>
        <span>Enter password: </span>
        <input type="text" v-on:input="SetPassword" />
    </p>
    <button v-on:click="SetJsonUser">Log in</button>      
  </div>
</template>

<script>
export default {  
  
  data () {
    return {                          
            nameUser: '',
            password: '' 
    }
  },
  methods:{
        SetUserName: function(event){
            this.nameUser = event.target.value;
        }, 
        SetPassword: function(event){
            this.password = event.target.value;
        },             
        SetJsonUser: async function(){
            let url = 'https://localhost:44372/api/Home/5/Authorize';
            let user = {
                userName: this.nameUser,
                password: Number(this.password)                    
            };
            let response = await fetch(url, {
                method: 'POST',                    
                headers: {
                   'Content-Type': 'application/json;charset=utf-8'                                              
                },
                body: JSON.stringify(user)                   
            });             
            if (response.ok) { 
                if (response.status == 200) {
                    let json = await response.json();                  
                    this.$activeUserName = json.userName;
                    this.$activeUserId = json.id;
                } else {
                    alert("Invalid username or password, try again.");
                }               
            } else {
                alert("Error HTTP: " + response.status);
            }
        }  
    }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    
    button {
        margin: 30px 75px;
    }   

   
</style>
