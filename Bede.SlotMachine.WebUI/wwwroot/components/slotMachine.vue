<template>
    <div>              
        <h3 class="gray">Enter deposit and set stake size</h3>
        <div class="form-group margin-top_25px">
            <button class="btn btn-primary" v-on:click="enterDeposit">Deposit</button>
            <button class="btn btn-warning margin-left_30" v-on:click="startAgain">Start again</button>
            <input v-model="stake" class="form-control pull-right" type="number" placeholder="Enter stake" min="0" oninput="validity.valid||(value='');"/>
        </div>
        <table class="table" style="margin-top:45px">
            <thead>
            </thead>
            <tbody>
                <tr v-for="dimension in dimensions" :class="{'active win' : dimension.IsWin}">
                    <td v-for="symbol in dimension.Symbols">
                        <div v-if="spinning" class="cube">
                            <div class="front apple"></div>
                            <div class="back banana"></div>
                            <div class="top"></div>
                            <div class="bottom"></div>
                            <div class="left pineapple"></div>
                            <div class="right wyldecard"></div>
                        </div>                        
                        <div v-else :class="getSymbolClass(symbol.Type)"></div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="form-group row">
            <h3 class="pull-left">Balance: <span class="label label-info">{{balance}}</span></h3>
            <h3 class="pull-left margin-left_30">Win: <span class="label label-info">{{win}}</span></h3>
            <div class="pull-right"><button style="width:80px; height:64px;" class="btn btn-primary" :disabled="spinning" v-on:click="spin">Spin</button></div>
        </div>

        <deposit-modal ref="depositModal" @deposit="onDeposit"/>        

    </div>


</template>

<script>
import Noty from 'noty';
import axios from 'axios';
import DepositModal from './depositModal';

export default {
    components:{
        'deposit-modal': DepositModal
    },
    data: function(){
        return {
            deposit: 0,
            stake: 0,
            balance: 0,
            win: 0,
            // Just to not show empty table init with anything
            dimensions: [
                    {Symbols: [{Type: 1}, {Type: 2}, {Type: 2}]},
                    {Symbols: [{Type: 1}, {Type: 1}, {Type: 1}]},
                    {Symbols: [{Type: 1}, {Type: 1}, {Type: 1}]},
                ],
            spinning: false,
        };
    },
    created: function(){
        this.spinning = true
        var self = this;

        axios.get("/home/getLastSpin")
        .then(response => {
            var data = response.data;
            if (data.OK) {                        
                self.dimensions = data.Data.Dimensions;
                self.balance = data.Data.Balance;
                self.stake = data.Data.Stake;
                self.win = data.Data.Win;               
            }

            return axios.get("/home/getBalance");                                       
        })
        .then((response) => {
            var data = response.data;
            if(data.OK){
                if(data.Data == 0){
                    self.enterDeposit();    
                }else{
                    self.balance = data.Data;
                }            
                // always executed
                self.spinning = false;
            }
        })
        .catch(e => {
            console.log(e);
        })                       
    },
    watch:{
        stake: function(newVal, oldVal){
            this.enterStake(newVal);
        }
    },
    methods: {
        getSymbolClass: function(type){
            if(type == 1){
                return 'apple';
            }else if(type == 2){
                return 'banana'
            }else if(type == 3){
                return 'pineapple'
            }else if(type == 4){
                return 'wyldecard'
            }
        },
        onDeposit: function(amount){
            this.balance += amount;
            this.showMessage(amount + ' deposited!', 'warning');            
        },
        enterDeposit: function () {
            this.$refs.depositModal.show();               
        },
        startAgain: function(){
            var self = this;
            axios.post("/home/startAgain")
            .then(response => {       
                var data = response.data;
                if(data.OK){
                    self.showMessage(data.Message, 'info');
                    self.balance = 0;
                    self.stake = 0;
                    self.win = 0;
                    self.enterDeposit(); 
                }                    
            })
            .catch(e => {
                console.log(e);
            })              
        },
        enterStake: function(stake){
            axios.post("/home/enterStake", { stake: stake })
            .then(response => {          
            })
            .catch(e => {
                console.log(e);
            })             
        },
        spin: function(){
            if(this.balance - this.stake >= 0){
                this.spinning = true
            }

            var self = this;

            axios.get("/home/spin")
            .then(response => {
                var data = response.data;
                if (data.OK) {            
                    self.dimensions = data.Data.Dimensions;            
                    self.balance = data.Data.Balance;
                    self.stake = data.Data.Stake;
                    self.win = data.Data.Win;
                }else{
                    self.showMessage(data.Message);                        
                }            
            })
            .catch(e => {
                console.log(data);
            }) 
            .then(() => {
                // always executed
                setTimeout(() => {
                    self.spinning = false;
                }, 500);
            }) 
        },
        showMessage: function(msg, type = 'error'){
            new Noty({
                theme: 'relax',
                text: msg,
                timeout: 1500,
                type: type
            }).show();
        }
    }  
}
</script>

