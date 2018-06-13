import SlotMachine from './wwwroot/components/slotMachine.vue';
import VModal from 'vue-js-modal'

var Vue = require('vue');

Vue.use(VModal, { dialog: true });

new Vue({
    el: '#mainApp',
    components:{
        'slot-machine' : SlotMachine
    }
});