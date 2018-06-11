<template> 
    <div>
        <h3 class="gray">Enter deposit and set stake size</h3>

        <div class="form-group margin-top_25px">
            <label for="deposit">Deposit:</label>
            <input type="number" class="form-control inline-block" v-model.number="deposit" id="deposit">
            <button v-on:click="enterDeposit" class="btn btn-primary inline-block">Enter</button>
        </div>
        <table class="table" style="margin-top:45px">
            <thead>
            </thead>
            <tbody>
                <tr v-for="dimension in spinResult.Dimensions" :class="{'active win' : dimension.IsWin}">
                    <td v-for="symbol in dimension.Symbols">
                        <div :class="getSymbolClass(symbol.Type)"></div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div>
            <h3 class="col-lg-2">Balance: <span class="label label-info">{{balance}}</span></h3>
            <h3 class="col-lg-3">
                <!-- Stake: <a @click="changeStake" class="label label-info">{{stake}}</a> -->
                <div class="input-group">
                    <span class="input-group-addon">Stake</span>
                    <input id="stake" v-model="stake" type="number" class="form-control" name="stake" placeholder="Enter stake">
                </div>
            </h3>
            <h3 class="col-lg-3">Win: <span class="label label-info">{{spinResult.Win}}</span></h3>
            <div><button class="btn btn-primary" :disabled="spinning" v-on:click="spin">Spin</button></div>
        </div>
    </div>
</template>

<script>
import Noty from 'noty';
import axios from 'axios';

export default {
    data: function(){
        return {
            deposit: 0,
            stake: 0,
            balance: 0,
            spinning: false,
            spinResult:  {}
        };
    },
    created: function(){
        this.spinning = true
        var self = this;

        axios.get("/home/getLastSpin")
        .then(response => {
            var data = response.data;
            if (data.OK) {                        
                self.spinResult = data.Data;
                self.balance = self.spinResult.Balance;
                self.stake = self.spinResult.Stake;
            }else{
                self.showMessage(data.Message);                        
            }            
        })
        .catch(e => {
            console.log(data);
        }) 
        .then(() => {
            // always executed
            self.spinning = false;
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
        enterDeposit: function () {
            var self = this;
            var jqxhr = $.post("/home/enterDeposit", { deposit: self.deposit }, function () {
                self.balance += self.deposit;
                self.showMessage(self.deposit + ' deposited!', 'info');
            })
            .fail(function (data) {
                console.log(data)
            })
            .always(function () {
                self.deposit = 0;
            });                
        },
        changeStake: function(){
            this.enterStake(20);
            self.stake = 20;
        },
        enterStake: function(stake){
            var self = this;
            var jqxhr = $.post("/home/enterStake", { stake: stake }, function () {

            })
            .fail(function (data) {
                console.log(data)
            })
            .always(function () {
                //self.stake = 0;
            });
        },
        spin: function(){
            this.spinning = true
            var self = this;

            var jqxhr = $.get( "/home/spin", function(data) {
                if (data.OK) {                        
                    self.spinResult = data.Data;
                    self.balance = self.spinResult.Balance;
                    self.stake = self.spinResult.Stake;
                }else{
                    self.showMessage(data.Message);                        
                }
            })
            .fail(function(data) {
                self.spinResult = {};
                console.log(data);
            })
            .always(function() {
                self.spinning = false;
            });
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

<style>

    .gray{
        color: gray;
    }
    .win{
        border: 2px solid red;
    }
    .banana {
        background-size: 32px 32px;
        background-repeat: no-repeat;
        background-image: url(data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTkuMC4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iQ2FwYV8xIiB4PSIwcHgiIHk9IjBweCIgdmlld0JveD0iMCAwIDUxMi4wMDEgNTEyLjAwMSIgc3R5bGU9ImVuYWJsZS1iYWNrZ3JvdW5kOm5ldyAwIDAgNTEyLjAwMSA1MTIuMDAxOyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSIgd2lkdGg9IjUxMnB4IiBoZWlnaHQ9IjUxMnB4Ij4KPHBhdGggc3R5bGU9ImZpbGw6I0E2RUQ5QTsiIGQ9Ik0zNzQuNjU0LDEwMy4zOTZsLTIyLjY5Mi02MC4zN2MtMS41NTgtNC4xNDUsMC44MDktOC43MjgsNS4wOTEtOS44NTdsNTEuMTE1LTEzLjQ4NiAgYzMuOTkxLTEuMDUzLDguMDgsMS4zMjgsOS4xMzQsNS4zMTlsMTYuOTgyLDY0LjI4NEwzNzQuNjU0LDEwMy4zOTZ6Ii8+CjxwYXRoIHN0eWxlPSJmaWxsOiNGRkM2NkM7IiBkPSJNNDM0LjI4NSw4OS4yODZsLTU5LjYzMiwxNC4xMWwtMTMuNDY0LDY0Ljk4OGMtOS41MjgsNDUuOTk1LTMxLjE4Miw4OC42MDUtNjIuNzE3LDEyMy40MTdsMCwwICBjLTM4LjUyMiw0Mi41MjUtODkuODc1LDcxLjMxLTE0Ni4yNTQsODEuOTc5bC00My41NDEsOC4yMzljLTE4LjA4MiwzLjQyMS0zNC45NjQsMTEuNDg2LTQ4Ljk5LDIzLjM5OWwwLDBsLTM4LjI5Nyw3LjI0N0wxMCw0NTEuOTggIGwwLDBjMzQuODk1LDIxLjMzNCw3NC4yMjcsMzQuMzYsMTE0Ljk1NywzOC4wN2wxMS4zMDQsMS4wM2M4NC4yNTcsNy42NzYsMTY4LjU0Ny0xNC4zNDEsMjM4LjI4Ni02Mi4yNDJsMCwwICBjNDkuOTEzLTM0LjI4Myw4OC43MzEtODEuNDk4LDExMi40OC0xMzYuNTQ3bDIuMTgzLTUuMDZjMjQuMzAzLTU2LjMzNiwxMy42MDUtMTIxLjY2My0yNy4zOTktMTY3LjMwNEw0MzQuMjg1LDg5LjI4NnoiLz4KPHBhdGggZD0iTTE1Mi4yMDksMzgzLjc4M2MwLjYyMywwLDEuMjU3LTAuMDU5LDEuODkyLTAuMTgxbDAuODI3LTAuMTU4YzUuNDI1LTEuMDM4LDguOTgtNi4yNzgsNy45NDEtMTEuNzA0ICBjLTEuMDM5LTUuNDI1LTYuMjc0LTguOTg3LTExLjcwNC03Ljk0MWwtMC44MjcsMC4xNThjLTUuNDI1LDEuMDM4LTguOTgsNi4yNzgtNy45NDEsMTEuNzA0ICBDMTQzLjMxNCwzODAuNDUxLDE0Ny41MDYsMzgzLjc4MywxNTIuMjA5LDM4My43ODN6Ii8+CjxwYXRoIGQ9Ik00NjkuMjUzLDExMy4yNDNsLTI1LjkxNS0yOC44NDVsLTE2LjM2NS02MS45NWMtMi40NjEtOS4zMTQtMTIuMDQxLTE0Ljg5NC0yMS4zNTUtMTIuNDM1bC01MS4xMTUsMTMuNDg1ICBjLTQuNzcyLDEuMjU5LTguODA2LDQuNDk2LTExLjA3LDguODc5Yy0yLjI2NSw0LjM4NS0yLjU2Nyw5LjU0OC0wLjgzMSwxNC4xNjhsMjEuNjcyLDU3LjY1OGwtMTIuODc3LDYyLjE1MiAgYy05LjE1OSw0NC4yMTItMzAuMDIzLDg1LjI2OC02MC4zMzYsMTE4LjczYy0yNy40MSwzMC4yNTktNjIuMTU2LDUzLjY5MS0xMDAuNDgxLDY3Ljc2Yy01LjE4NiwxLjkwMy03Ljg0Niw3LjY1LTUuOTQyLDEyLjgzNiAgYzEuOTA0LDUuMTg2LDcuNjUxLDcuODQ0LDEyLjgzNiw1Ljk0MmM0MS4zNTEtMTUuMTgxLDc4LjgzOC00MC40NjEsMTA4LjQxMS03My4xMDhjMzIuNzA2LTM2LjEwNCw1NS4yMTYtODAuNCw2NS4wOTgtMTI4LjEwMSAgbDEyLjE3Mi01OC43NTFsNDcuNjU2LTExLjI3N2wyMy41NjIsMjYuMjI2YzM4LjI2Niw0Mi41OTIsNDguMzM3LDEwNC4wODQsMjUuNjU2LDE1Ni42NThsLTIuMTgzLDUuMDYxICBjLTIzLjAzMiw1My4zODgtNjAuNzEsOTkuMTIzLTEwOC45NiwxMzIuMjY0Yy02Ny42NjksNDYuNDgxLTE0OS45NTUsNjcuOTc0LTIzMS43MTYsNjAuNTI3bC0xMS4zMDQtMS4wMjkgIGMtMTQuNTEyLTEuMzIyLTI4LjkxOS0zLjkyOS00My4wMTctNy43MDdjMy42MTEtOS40MzYsNS40OTYtMTkuNDk5LDUuNDk2LTI5LjY0OXYtMy4wNjNjMC01LjUyMy00LjQ3OS0xMC4wMDEtMTAuMDAxLTEwLjAwMSAgcy0xMC4wMDEsNC40NzktMTAuMDAxLDEwLjAwMXYzLjA2M2MwLDguMTQtMS41OTEsMTYuMjA4LTQuNjQ1LDIzLjcxNmMtMTQuNTE2LTUuMTIyLTI4LjU4MS0xMS41MTYtNDEuOTMyLTE5LjEzNWw3LjUyMi0yNS45NjUgIGwzMi4yNjEtNi4xMDVjMS43MDMtMC4zMjIsMy4yOTQtMS4wODEsNC42MTUtMi4yMDRjMTIuNjgyLTEwLjc3MywyOC4wMjYtMTguMTAxLDQ0LjM3NC0yMS4xOTQgIGM1LjQyOC0xLjAyNiw4Ljk5NC02LjI1OSw3Ljk2OC0xMS42ODdjLTEuMDI3LTUuNDI3LTYuMjU4LTguOTk1LTExLjY4OC03Ljk2N2MtMTguODY0LDMuNTY4LTM2LjYyMSwxMS44MDctNTEuNTI2LDIzLjg3OCAgbC0zNS43NjEsNi43NjhjLTMuNzA0LDAuNy02LjY5OCwzLjQyMy03Ljc0Nyw3LjA0NEwwLjM5Niw0NDkuMTk4Yy0xLjI1NCw0LjMzLDAuNTQzLDguOTY0LDQuMzkxLDExLjMxNiAgYzE4Ljg0MSwxMS41MTksMzkuMTE4LDIwLjc0NSw2MC4xOSwyNy40NzdjMC41MjYsMC4yMiwxLjA2MywwLjM4NCwxLjYwNSwwLjUwOWMxOC42OTQsNS44NDksMzcuOTk4LDkuNzM3LDU3LjQ3MiwxMS41MTIgIGwxMS4zMDQsMS4wMjljMTEuMjIxLDEuMDIxLDIyLjQzNywxLjUyOCwzMy42NDIsMS41MjhjNzUuMDY0LTAuMDAzLDE0OC45OTUtMjIuNzUxLDIxMS4yMTYtNjUuNDg3ICBjNTEuMzYxLTM1LjI3OCw5MS40NzItODMuOTc2LDExNS45OTktMTQwLjgzbDIuMTgzLTUuMDYxQzUyNC4xNTksMjMxLjQ3NCw1MTIuNzIxLDE2MS42MjUsNDY5LjI1MywxMTMuMjQzeiBNNDA4LjI3NywyOS45OTggIGwxMy43MTYsNTEuOTE4bC00MS4wNzUsOS43MTlsLTE4LjYxLTQ5LjUxTDQwOC4yNzcsMjkuOTk4eiIvPgo8cGF0aCBkPSJNNDQyLjI5MywyODQuMjY4YzMuODU4LDAsNy41MzQtMi4yNDYsOS4xNzQtNi4wMWwwLjQ4Ni0xLjEyMmMyLjE3Ny01LjA3Ny0wLjE3My0xMC45NTctNS4yNDktMTMuMTM1ICBjLTUuMDc5LTIuMTc1LTEwLjk1NywwLjE3My0xMy4xMzUsNS4yNDlsLTAuNDQxLDEuMDE5Yy0yLjIwNiw1LjA2NCwwLjExMSwxMC45NTgsNS4xNzUsMTMuMTY0ICBDNDM5LjYwMywyODQsNDQwLjk1OSwyODQuMjY4LDQ0Mi4yOTMsMjg0LjI2OHoiLz4KPHBhdGggZD0iTTM0Ny4yNjMsMzk5LjEwMmMxLjk1MSwwLDMuOTI0LTAuNTcsNS42NTQtMS43NThjMjkuMTEyLTE5Ljk5Nyw1My44Ny00NC44NDMsNzMuNTg2LTczLjg1ICBjMy4xMDQtNC41NjgsMS45MTktMTAuNzg5LTIuNjQ5LTEzLjg5NGMtNC41NjktMy4xMDUtMTAuNzg5LTEuOTE5LTEzLjg5NCwyLjY0OWMtMTguMzEyLDI2Ljk0MS00MS4zMTQsNTAuMDIzLTY4LjM2OCw2OC42MDUgIGMtNC41NTMsMy4xMjctNS43MDksOS4zNTQtMi41ODEsMTMuOTA3QzM0MC45NDksMzk3LjU4NCwzNDQuMDc5LDM5OS4xMDIsMzQ3LjI2MywzOTkuMTAyeiIvPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8L3N2Zz4K);
        width: 32px;
        height: 32px;
    }

    .apple {
        background-size: 32px 32px;
        background-repeat: no-repeat;
        background-image: url(data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTguMC4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjwhRE9DVFlQRSBzdmcgUFVCTElDICItLy9XM0MvL0RURCBTVkcgMS4xLy9FTiIgImh0dHA6Ly93d3cudzMub3JnL0dyYXBoaWNzL1NWRy8xLjEvRFREL3N2ZzExLmR0ZCI+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iQ2FwYV8xIiB4PSIwcHgiIHk9IjBweCIgdmlld0JveD0iMCAwIDU2LjY3MyA1Ni42NzMiIHN0eWxlPSJlbmFibGUtYmFja2dyb3VuZDpuZXcgMCAwIDU2LjY3MyA1Ni42NzM7IiB4bWw6c3BhY2U9InByZXNlcnZlIiB3aWR0aD0iNTEycHgiIGhlaWdodD0iNTEycHgiPgo8Zz4KCTxwYXRoIHN0eWxlPSJmaWxsOiNEMTM4MzQ7IiBkPSJNMzEuNDgzLDE2LjIxM2MtMi4wNjUsMC41MTEtNC4yMjcsMC41MTEtNi4yOTIsMGMtMTAuNzU2LTIuNjYtMTkuNzYyLTEuNjQtMTkuNjU4LDE4LjY0MyAgIGMwLjA1MywxMC4zOSw5Ljc4MywyMy44NjIsMjAuMDA5LDIxLjU1N2MxLjgzNC0wLjQxMywzLjc1NS0wLjQxMyw1LjU4OSwwYzEwLjIyNiwyLjMwNSwxOS45NTYtMTEuMTY3LDIwLjAwOS0yMS41NTcgICBDNTEuMjQ0LDE0LjU3NCw0Mi4yMzksMTMuNTUzLDMxLjQ4MywxNi4yMTN6Ii8+Cgk8cGF0aCBzdHlsZT0iZmlsbDojRjc1QjU3OyIgZD0iTTEwLjUzMywyOS42NzRjLTAuNTUzLDAtMS0wLjQ0OC0xLTFjMC01LjUxNCw0LjAzNy0xMCw5LTEwYzAuNTUzLDAsMSwwLjQ0OCwxLDFzLTAuNDQ3LDEtMSwxICAgYy0zLjg1OSwwLTcsMy41ODktNyw4QzExLjUzMywyOS4yMjYsMTEuMDg1LDI5LjY3NCwxMC41MzMsMjkuNjc0eiIvPgoJPHBhdGggc3R5bGU9ImZpbGw6IzRDMzEyQzsiIGQ9Ik0yOC4yNjUsMTkuOTk5Yy0wLjUzNywwLTAuOTgxLTAuNDI3LTAuOTk4LTAuOTY4Yy0wLjEzNS00LjIzMi0xLjc2MS05LjU0NS01LjkyMi0xMS43NjQgICBjLTAuNDg3LTAuMjYtMC42NzItMC44NjUtMC40MTItMS4zNTNjMC4yNjEtMC40ODcsMC44NjQtMC42NzIsMS4zNTQtMC40MTJjNC45MTQsMi42Miw2LjgyNyw4LjY3Myw2Ljk3OSwxMy40NjUgICBjMC4wMTgsMC41NTItMC40MTUsMS4wMTQtMC45NjgsMS4wMzFDMjguMjg2LDE5Ljk5OSwyOC4yNzYsMTkuOTk5LDI4LjI2NSwxOS45OTl6Ii8+Cgk8cGF0aCBzdHlsZT0iZmlsbDojOTk0NTMwOyIgZD0iTTI4LjE3NiwyMy41OTJjLTEuNjgsMC0zLjI2MS0wLjY1NS00LjQ1LTEuODQ1Yy0wLjM5MS0wLjM5LTAuMzkxLTEuMDIzLDAtMS40MTQgICBzMS4wMjMtMC4zOTEsMS40MTQsMGMwLjgxMiwwLjgxMiwxLjg5MSwxLjI1OSwzLjAzNiwxLjI1OXMyLjIyNC0wLjQ0NywzLjAzNS0xLjI1OWMwLjM5MS0wLjM5MSwxLjAyMy0wLjM5MSwxLjQxNCwwICAgYzAuMzkxLDAuMzksMC4zOTEsMS4wMjMsMCwxLjQxNEMzMS40MzYsMjIuOTM3LDI5Ljg1NiwyMy41OTIsMjguMTc2LDIzLjU5MnoiLz4KCTxwYXRoIHN0eWxlPSJmaWxsOiM2NTlDMzU7IiBkPSJNMjcuMzA1LDEzLjA0bDAuNjA5LTQuMDg2YzAuNjQzLTQuMzE1LDQuMDMxLTcuNzAzLDguMzQ2LTguMzQ2TDQwLjM0NSwwbC0wLjYwOSw0LjA4NiAgIGMtMC42NDMsNC4zMTUtNC4wMzEsNy43MDMtOC4zNDYsOC4zNDZMMjcuMzA1LDEzLjA0eiIvPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+Cjwvc3ZnPgo=);
        width: 32px;
        height: 32px;
    }

    .pineapple {
        background-size: 32px 32px;
        background-repeat: no-repeat;
        background-image: url(data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTguMC4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjwhRE9DVFlQRSBzdmcgUFVCTElDICItLy9XM0MvL0RURCBTVkcgMS4xLy9FTiIgImh0dHA6Ly93d3cudzMub3JnL0dyYXBoaWNzL1NWRy8xLjEvRFREL3N2ZzExLmR0ZCI+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iQ2FwYV8xIiB4PSIwcHgiIHk9IjBweCIgdmlld0JveD0iMCAwIDU3LjA2IDU3LjA2IiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCA1Ny4wNiA1Ny4wNjsiIHhtbDpzcGFjZT0icHJlc2VydmUiIHdpZHRoPSI1MTJweCIgaGVpZ2h0PSI1MTJweCI+CjxnPgoJPHBhdGggc3R5bGU9ImZpbGw6I0Q2QTU1MDsiIGQ9Ik0yOC43NTgsMTkuMDZjMTYuMDU4LDAsMTUuOTQ0LDguNTA3LDE1Ljk0NCwxOXMtNy4xMzksMTktMTUuOTQ0LDE5aC0wLjQ1NiAgIGMtOC44MDYsMC0xNS45NDQtOC41MDctMTUuOTQ0LTE5cy0wLjExNC0xOSwxNS45NDQtMTkiLz4KCTxnPgoJCTxwYXRoIHN0eWxlPSJmaWxsOiM4OEMwNTc7IiBkPSJNMzIuNTEsMTMuMzg5YzAuNTE1LTAuNjQ0LDEuMDk1LTEuMjQ2LDEuNzI4LTEuNzdjMC43OTEtNC41Niw2LjIzNC04LjI3OCwxMC4xMTktOS4yOCAgICBjMCwwLTMuMzE4LTIuNzYtNy40MzMtMi4yODRjLTQuMjY0LDAuNDkzLTYuNjg4LDIuNTg0LTguMDU1LDUuMDU2QzMxLjc1LDcuMzYxLDMyLjQyOSwxMC45MDQsMzIuNTEsMTMuMzg5eiIvPgoJPC9nPgoJPHBhdGggc3R5bGU9ImZpbGw6IzY1OUMzNTsiIGQ9Ik0yMy4yNTUsMTkuNDExYzEuNDc2LTAuMjI4LDMuMTQ1LTAuMzUxLDUuMDQ4LTAuMzUxaDAuMDU2YzAsMCwwLjE2Ny01LjA0Mi01LTYgICBjLTUuMTY3LTAuOTU4LTYsMi02LDJDMjEuODI1LDE2LjEzMSwyMi45NjMsMTguMzIxLDIzLjI1NSwxOS40MTF6Ii8+Cgk8cGF0aCBzdHlsZT0iZmlsbDojNjU5QzM1OyIgZD0iTTM0LjYxMiwxOS41NTNjMC44MTktMS41NTIsNC42OC01LjI3OSw5Ljc0Ni02LjQ5M2MwLDAtMS44MzMtMy45NTgtNy0zICAgYy01LjE2NywwLjk1OC03Ljk5OSw5LTcuOTk5LDlzMC4wMjgsMC4wMDUsMC4wNjksMC4wMTFDMzEuMzk5LDE5LjEwMiwzMy4xMTcsMTkuMjY3LDM0LjYxMiwxOS41NTN6Ii8+Cgk8cGF0aCBzdHlsZT0iZmlsbDojQTRFODY5OyIgZD0iTTI1Ljk4OCwxNC4xNTZjMi4yNiwxLjY1MiwyLjM2Nyw0LjM5OSwyLjM3LDQuODM4YzAuMzM1LDAuMDE2LDAuNjc5LDAuMDE0LDEuMDI1LDAgICBjMC4xNDUtMC4zOTUsMS4wMjEtMi42ODIsMi41NjUtNC44NTljMC4wNTktMC4xMTUsMC4xMTktMC4yMDQsMC4xODEtMC4yNDJjMC4wNzQtMC4xMDEsMC4xNTItMC4yMDEsMC4yMjgtMC4zMDEgICBjMC4wNTItMC4wNjcsMC4xLTAuMTM2LDAuMTU0LTAuMjAzYy0wLjExOS0zLjYyNi0xLjUwNi05LjUwNy05LjE1My0xMC4zMjljLTMuODc1LTAuNDE3LTcsMi03LDIgICBDMjAuMzA5LDYuMDA3LDI1Ljk4Myw5LjcyNywyNS45ODgsMTQuMTU2TDI1Ljk4OCwxNC4xNTZ6Ii8+Cgk8Zz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIyMS4zNTgsNDQuNjQ2IDE3LjY1MSw0OC4zNTMgMTkuMDY1LDQ5Ljc2NyAyMS4zNTgsNDcuNDc0IDIzLjY1MSw0OS43NjcgMjUuMDY1LDQ4LjM1MyAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIyNC42NTEsNTMuMzUzIDI2LjA2NSw1NC43NjcgMjguMzU4LDUyLjQ3NCAzMC42NTEsNTQuNzY3IDMyLjA2NSw1My4zNTMgMjguMzU4LDQ5LjY0NiAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIzMS42NTEsNDguMzUzIDMzLjA2NSw0OS43NjcgMzUuMzU4LDQ3LjQ3NCAzNy42NTEsNDkuNzY3IDM5LjA2NSw0OC4zNTMgMzUuMzU4LDQ0LjY0NiAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIxOS4wNjUsNDEuNzY3IDIxLjM1OCwzOS40NzQgMjMuNjUxLDQxLjc2NyAyNS4wNjUsNDAuMzUzIDIxLjM1OCwzNi42NDYgMTcuNjUxLDQwLjM1MyAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIyOC4zNTgsNDMuNDc0IDMwLjY1MSw0NS43NjcgMzIuMDY1LDQ0LjM1MyAyOC4zNTgsNDAuNjQ2IDI0LjY1MSw0NC4zNTMgMjYuMDY1LDQ1Ljc2NyAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIxNC40OTgsNDAuNjQ2IDEyLjc5MSw0Mi4zNTMgMTQuMjA1LDQzLjc2NyAxNC40OTgsNDMuNDc0IDE2Ljc5MSw0NS43NjcgMTguMjA1LDQ0LjM1MyAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIzNS4zNTgsMzkuNDc0IDM3LjY1MSw0MS43NjcgMzkuMDY1LDQwLjM1MyAzNS4zNTgsMzYuNjQ2IDMxLjY1MSw0MC4zNTMgMzMuMDY1LDQxLjc2NyAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIxNy42NTEsMzIuMzUzIDE5LjA2NSwzMy43NjcgMjEuMzU4LDMxLjQ3NCAyMy42NTEsMzMuNzY3IDI1LjA2NSwzMi4zNTMgMjEuMzU4LDI4LjY0NiAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIyOC4zNTgsMzUuNDc0IDMwLjY1MSwzNy43NjcgMzIuMDY1LDM2LjM1MyAyOC4zNTgsMzIuNjQ2IDI0LjY1MSwzNi4zNTMgMjYuMDY1LDM3Ljc2NyAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIzNS4zNTgsMjguNjQ2IDMxLjY1MSwzMi4zNTMgMzMuMDY1LDMzLjc2NyAzNS4zNTgsMzEuNDc0IDM3LjY1MSwzMy43NjcgMzkuMDY1LDMyLjM1MyAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIyMS4zNTgsMjMuNDc0IDIzLjY1MSwyNS43NjcgMjUuMDY1LDI0LjM1MyAyMS4zNTgsMjAuNjQ2IDE3LjY1MSwyNC4zNTMgMTkuMDY1LDI1Ljc2NyAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIyNi4wNjUsMjkuNzY3IDI4LjM1OCwyNy40NzQgMzAuNjUxLDI5Ljc2NyAzMi4wNjUsMjguMzUzIDI4LjM1OCwyNC42NDYgMjQuNjUxLDI4LjM1MyAgICAgICAiLz4KCQk8cG9seWdvbiBzdHlsZT0iZmlsbDojRjRDNDRFOyIgcG9pbnRzPSIzNS4zNTgsMjMuNDc0IDM3LjY1MSwyNS43NjcgMzkuMDY1LDI0LjM1MyAzNS4zNTgsMjAuNjQ2IDMxLjY1MSwyNC4zNTMgMzMuMDY1LDI1Ljc2NyAgICAgICAiLz4KCQk8cGF0aCBzdHlsZT0iZmlsbDojRjRDNDRFOyIgZD0iTTQzLjIxMiwyNS41MzhsLTAuNjk5LTEuMDQ4bC0zLjg2MiwzLjg2M2wxLjQxNCwxLjQxNGwyLjEzOC0yLjEzN2wxLjMyMywxLjk4NWwwLjczMi0wLjQ4OCAgICBDNDQuMDM4LDI3LjgzMyw0My43MDUsMjYuNjMxLDQzLjIxMiwyNS41Mzh6Ii8+CgkJPHBhdGggc3R5bGU9ImZpbGw6I0Y0QzQ0RTsiIGQ9Ik00Mi4xNiw0MC44NDNsLTMuNTEsMy41MWwxLjQxNCwxLjQxNGwyLjQ5LTIuNDlsMS4zNTUsMC42NzhjMC4xOC0wLjY1NiwwLjMzNi0xLjMyMywwLjQ1Ni0yLjAwOCAgICBMNDIuMTYsNDAuODQzeiIvPgoJCTxwYXRoIHN0eWxlPSJmaWxsOiNGNEM0NEU7IiBkPSJNMTQuMjMsMjQuNzc0Yy0wLjM4MywwLjY5My0wLjY4OCwxLjQzOC0wLjkzLDIuMjI4bDAuNzY1LDAuNzY1bDAuMjkzLTAuMjkzbDIuMjkzLDIuMjkzICAgIGwxLjQxNC0xLjQxNGwtMy43MDctMy43MDdMMTQuMjMsMjQuNzc0eiIvPgoJCTxwYXRoIHN0eWxlPSJmaWxsOiNGNEM0NEU7IiBkPSJNNDQuNjg5LDM0Ljk3N2wtMi4zMzEtMi4zMzFsLTMuNzA3LDMuNzA3bDEuNDE0LDEuNDE0bDIuMjkzLTIuMjkzbDEuMjkzLDEuMjkzbDEuMDQ2LTEuMDQ2ICAgIEM0NC42OTUsMzUuNDcyLDQ0LjY5MiwzNS4yMjMsNDQuNjg5LDM0Ljk3N3oiLz4KCQk8cGF0aCBzdHlsZT0iZmlsbDojRjRDNDRFOyIgZD0iTTEyLjM3OCwzNC42MjZjLTAuMDA4LDAuNDc0LTAuMDE0LDAuOTUyLTAuMDE3LDEuNDM3bDAuNzA0LDAuNzA0bDEuMjkzLTEuMjkzbDIuMjkzLDIuMjkzICAgIGwxLjQxNC0xLjQxNGwtMy43MDctMy43MDdMMTIuMzc4LDM0LjYyNnoiLz4KCTwvZz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8L3N2Zz4K);
        width: 32px;
        height: 32px;
    }

    .wyldecard {
        background-size: 32px 32px;
        background-repeat: no-repeat;
        background-image: url(data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTkuMC4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iQ2FwYV8xIiB4PSIwcHgiIHk9IjBweCIgdmlld0JveD0iMCAwIDQ3Ljk0IDQ3Ljk0IiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCA0Ny45NCA0Ny45NDsiIHhtbDpzcGFjZT0icHJlc2VydmUiIHdpZHRoPSI1MTJweCIgaGVpZ2h0PSI1MTJweCI+CjxwYXRoIHN0eWxlPSJmaWxsOiNFRDhBMTk7IiBkPSJNMjYuMjg1LDIuNDg2bDUuNDA3LDEwLjk1NmMwLjM3NiwwLjc2MiwxLjEwMywxLjI5LDEuOTQ0LDEuNDEybDEyLjA5MSwxLjc1NyAgYzIuMTE4LDAuMzA4LDIuOTYzLDIuOTEsMS40MzEsNC40MDNsLTguNzQ5LDguNTI4Yy0wLjYwOCwwLjU5My0wLjg4NiwxLjQ0OC0wLjc0MiwyLjI4NWwyLjA2NSwxMi4wNDIgIGMwLjM2MiwyLjEwOS0xLjg1MiwzLjcxNy0zLjc0NiwyLjcyMmwtMTAuODE0LTUuNjg1Yy0wLjc1Mi0wLjM5NS0xLjY1MS0wLjM5NS0yLjQwMywwbC0xMC44MTQsNS42ODUgIGMtMS44OTQsMC45OTYtNC4xMDgtMC42MTMtMy43NDYtMi43MjJsMi4wNjUtMTIuMDQyYzAuMTQ0LTAuODM3LTAuMTM0LTEuNjkyLTAuNzQyLTIuMjg1bC04Ljc0OS04LjUyOCAgYy0xLjUzMi0xLjQ5NC0wLjY4Ny00LjA5NiwxLjQzMS00LjQwM2wxMi4wOTEtMS43NTdjMC44NDEtMC4xMjIsMS41NjgtMC42NSwxLjk0NC0xLjQxMmw1LjQwNy0xMC45NTYgIEMyMi42MDIsMC41NjcsMjUuMzM4LDAuNTY3LDI2LjI4NSwyLjQ4NnoiLz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPC9zdmc+Cg==);
        width: 32px;
        height: 32px;
    }

    .margin-top_25px{
        margin-top: 25px;
    }
    .inline-block{
        display: inline-block;
    }
</style>

