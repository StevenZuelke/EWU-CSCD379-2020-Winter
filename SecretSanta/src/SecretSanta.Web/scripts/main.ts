import '../styles/site.scss';
import { App } from './app';
import Vue from 'vue';

import Blah from './blah.vue';
if (document.getElementById('blah')) {
    new Vue({
        render: h => h(Blah)
    }).$mount('#blah');
}

if (document.getElementById('userList')) {
    new Vue({
        render: h => h(UsersComponent)
    }).$mount('#userList');
}

import { Gift } from './secretsanta-client';
import UsersComponent from './components/User/userComponent.vue';

document.addEventListener("DOMContentLoaded", async () => {
    let app = new App.Main();
    if (document.getElementById('giftList')) {
        await app.deleteGifts();

        await app.createUser();

        await app.createGifts();

        let gifts = await app.getGifts();

        let element = document.getElementById('giftList');

        for (let gift of gifts) {
            let liElement = element.appendChild(document.createElement('li'));
            liElement.textContent = `${gift.id} ${gift.title} ${gift.description} ${gift.url}`;
        }
    }
    
     
});