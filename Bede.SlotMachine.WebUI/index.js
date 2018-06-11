import SlotMachine from './wwwroot/components/slotMachine.vue';

var Vue = require('vue');

new Vue({
    el: '#mainApp',
    components:{
        'slot-machine' : SlotMachine
    }
});