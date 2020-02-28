import '../styles/site.scss';
import { App } from './app';
import Vue from 'vue';


if (document.getElementById('userList')) {
    new Vue({
        render: h => h(UsersComponent)
    }).$mount('#userList');
}

if (document.getElementById('giftList')) {
    new Vue({
        render: h => h(GiftsComponent)
    }).$mount('#giftList');
}

if (document.getElementById('groupList')) {
    new Vue({
        render: h => h(GroupsComponent)
    }).$mount('#groupList');
}

import { Gift } from './secretsanta-client';
import UsersComponent from './components/User/userComponent.vue';
import GiftsComponent from './components/Gift/giftComponent.vue';
import GroupsComponent from './components/Group/groupComponent.vue';

document.addEventListener("DOMContentLoaded", async () => {

    let app = new App.Main();
    
});