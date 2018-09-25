<template>
  <modal name="depositModal"
         :classes="['v--modal', 'error-modal', !deposited && 'has-bugs']"
         :pivot-y="0.2"
         :width="400"
         :height="300"
         @before-open="beforeOpen"
         @before-close="beforeClose">
    <div class="error-modal-content">
      <div class="bugs-label">Deposit</div>
      <div style="padding: 10px;">
        You will be able to close the window only if you deposit :)
      </div>
      <input type="number" v-model.number="depositAmount" min="1" placeholder="Enter deposit" oninput="validity.valid||(value='');"/>
      <button @click="deposit">Confirm</button>
    </div>
  </modal>
</template>

<script>
import axios from 'axios';

export default {
  name: 'depositModal',
  data () {
    return {
      deposited: false,
      depositAmount: null
    }
  },
  methods: {
    show: function(){
        this.$modal.show('depositModal');
    },
    getDepositAmount: function(){
        return this.depositAmount;
    },
    deposit () {
      if(!(this.depositAmount && this.depositAmount > 0)){
        return;
      }

      var self = this;
        axios.post("/home/enterDeposit", { deposit: self.depositAmount })
        .then(response => {  
          self.$emit('deposit', self.depositAmount);
          self.deposited = true;
          self.depositAmount = null;
          self.$modal.hide('depositModal')                  
        })
        .catch(e => {
            console.log(data);
        })      
    },
    beforeOpen (event) {
      this.deposited = false;
    },
    beforeClose (event) {
      if (!this.deposited) {
        /*
        Stopping close event execution
        */
        event.stop()
      }
    }
  }
}
</script>

<style lang="scss" scoped>
  .error-modal {
    transition: box-shadow 1s;
    &.has-bugs {
      box-shadow: 0 24px 80px -2px rgba(255, 0, 0, .6) !important;
    }
  }
  .error-modal-content {
    padding: 10px;
    text-align: center;
    .bugs-label {
      text-transform: uppercase;
      font-size: 60px;
      font-weight: 300;
      letter-spacing: 2px;
      padding: 40px;
    }
    button {
      width: 180px;
    }
    sub {
      color: #EC625F;
      transition: opacity 0.25s;
    }
  }
</style>
