# OrderCloud-api Message Sender
###### A .NET WebAPI to demonstrate handling message send events from the OrderCloud API
OrderCloud Message Sender is a feature that helps to deliver event driven notifications to users. Think of it as an enhanced web hook. If, for instance, you wanted to use web hooks to deliver a notification to all users who are able to approve a newly submitted order, you would have to write code to cover the following steps:

* A handler to recieve the submit for approval web hook
* take that high level order info and query the api for the rest of the needed order data
* query the api for all of the possible approving users which can be compelx
* lookup approving users' contact info
* send each message

Message senders will send one web request for each message that should be sent. Considering that you have subscribed to the events, on the action of submitting an order for approval, a notification will be sent for the user who submitted the order and any user who can approve it.

The easiest way to use message senders is to simply enter your Mandrill api key into dev center and use our built in integration. You can control your own templates and Mandrill account, but you don't have to write or host any code to handle the events. If our built in integrations aren't enough, this .NET project should give you a great starting point to implement your own message sender.

First, let's address security. You're going to be hosting an endpoint on the web that will deliver messages to your users, you want to at the very least verify that it is the OrderCloud platform that is sending the request. Check the request headers for X-OC-hash. The value will be a hash of the full http body using a key that you specify in the message sender config. Here's an example in c# of how to generate the hash:
```csharp
var fullHttpBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
var keyBytes = Encoding.UTF8.GetBytes(Configuration["OCHashKey"]);
var dataBytes = Encoding.UTF8.GetBytes(fullHttpBody);
var hmac = new HMACSHA256(keyBytes);
var hmacBytes = hmac.ComputeHash(dataBytes);
var hash = Convert.ToBase64String(hmacBytes);
```
If the var hash does not match the value in the http header, you have an unverified sender of that web request.

Each message sender request contains the following:
```javascript
{
	"BuyerID" : "", //populated depending on the messagetype
	"UserToken" : "", //an elevated access token incase your integration needs more data
	"Recipient" : {}, //user object of the recpient
	"MessageType": "", //one of the possible message types
	"ConfigData" : {}, //any extra JSON formatted data you need to make the integration work. Often, it's a good place to store your api key for any third party services, if you don't have a good place to save sensitive data on your server
	"EventBody": {}, //data need to construct the email. It's specific to the message type
}
```
From there it's up to your integration to send the notification to the end user in whatever way is required. We're always looking to add new message delivery partners, so if we don't currently support your preferred platform, just ask and we may add it to our list. Or better yet, code one into this project and submit a PR.

After your message sender is configured in dev center, you **must** assign it to the party that will be subscribed to it. The configuration tells OrderCloud where to send the notification, but the assignment tells OrderCloud **who** should get it. See message sender assignments in the console.

