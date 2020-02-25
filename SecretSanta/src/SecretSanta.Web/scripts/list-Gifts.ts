import {
    IGiftClient,
    IUserClient,
    UserClient,
    GiftClient,
    IGiftInput,
    GiftInput,
    UserInput,
    User,
    Gift
} from "./secretsanta-client";

export const hello = () => "Hello world!";

export class App {
    async renderGifts() {
        await this.deleteAllGifts();
        //await this.AddUserOne();
        await this.fillGifts();
        var gifts = await this.getAllGifts();
        const itemList = document.getElementById("giftList");
        for (let index = 0; index < gifts.length; index++) {
            const gift = gifts[index];
            //document.write("Hello World"); 
            const listItem = document.createElement("li");
            listItem.textContent = `${gift.title}:${gift.description}:${gift.url}`;
            itemList.append(listItem);
        }
    }

    giftClient: IGiftClient;
    userClient: IUserClient;
    userId: number;
    constructor(giftClient: IGiftClient = new GiftClient()) {
        this.giftClient = giftClient;
        this.userClient = new UserClient();
    }

    /*async AddUserOne() {
        var users = await this.getAllUsers();
        for (let index = 0; index < users.length; index++) {
            await this.userClient.delete(users[index].id);
        }
        var user = new UserInput();
        user.firstName = "FirstName";
        user.lastName = "LastName";
        await this.userClient.post(user);
        var userAll = await this.userClient.getAll();
        var userOne = userAll[1];
       // this.userId = userOne.id;
        this.userId = 10;
    }*/

    async deleteAllGifts() {
        var gifts = await this.getAllGifts();
        for (let index = 0; index < gifts.length; index++) {
            await this.giftClient.delete(gifts[index].id);
        }
    }

    async fillGifts() {
        var user = new User({
            firstName: "firstName",
            lastName: "lastName",
            santaId: null,
            gifts: null,
            groups: null,
            id: 1
        })
        const title = "Title ";
        const desc = "Description ";
        const url = "Url ";
        for (let index = 0; index < 10; index++) {
            var giftIn = new Gift();
            giftIn.title = title;
            giftIn.description = desc;
            giftIn.url = url;
            giftIn.userId = 1;
            giftIn.id = index;
            await this.giftClient.post(giftIn);
        }
    }

    async getAllUsers() {
        var users = await this.userClient.getAll();
        return users;
    }

    async getAllGifts() {
        var gifts = await this.giftClient.getAll();
        return gifts;
    }
}
