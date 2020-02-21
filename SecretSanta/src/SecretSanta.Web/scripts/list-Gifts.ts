import {
    IGiftClient,
    IUserClient,
    GiftClient,
    IGiftInput,
    GiftInput
} from "./secretsanta-client";

export const hello = () => "Hello world!";

export class App {
    async renderGifts() {
        await this.deleteAllGifts();
        await this.fillGifts();
        var gifts = await this.getAllGifts();
        const itemList = document.getElementById("giftList");
        for (let index = 0; index < gifts.length; index++) {
            const gift = gifts[index];
            document.write("Hello World"); 
            const listItem = document.createElement("li");
            listItem.textContent = `${gift.title}:${gift.description}:${gift.url}`;
            //itemList.append(listItem);
        }
    }

    giftClient: IGiftClient;
    constructor(giftClient: IGiftClient = new GiftClient()) {
        this.giftClient = giftClient;
    }

    async deleteAllGifts() {
        var gifts = await this.getAllGifts();
        for (let index = 0; index < gifts.length; index++) {
            await this.giftClient.delete(gifts[index].id);
        }
    }

    async fillGifts() {
        const title = "Title ";
        const desc = "Description ";
        const url = "Url ";
        for (let index = 0; index < 10; index++) {
            var giftIn = new GiftInput();
            giftIn.title = title + index;
            giftIn.description = desc + index;
            giftIn.url = desc + index;
            giftIn.userId = 1;
            await this.giftClient.post(giftIn);
        }
    }

    async getAllGifts() {
        var gifts = await this.giftClient.getAll();
        return gifts;
    }
}
