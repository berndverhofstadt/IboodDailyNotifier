
Ibood Notification App
===================

Wolcome to the ibood Notification App. 
You don't have to worry for missing your favorite product. This app is doing all the work for you. It's easy and free. You just have to add the keywords e.g. keywords you are looking for and add your IFTTT custom notification. That's it.

----------


![Ibood Notification center](https://raw.githubusercontent.com/berndverhofstadt/IboodDailyNotifier/master/images/ibood_notification_center.PNG)


#### Notifications ####

Notification will be send by an IFTTT trigger. You have to activate an <a href="https://ifttt.com/maker">IFTTT Maker</a> event trigger. Fill in the eventName and enter the key.

#### Create an IFTTT account

If you already have an IFTTT account, skip this step.
Go to https://ifttt.com/join and create account.

#### Create an IFTTT Notification Trigger

When you're logged in to your IFTTT account. 
Go to [https://ifttt.com/maker](https://ifttt.com/maker). If you don't have one, login at [https://ifttt.com/login](https://ifttt.com/login)
Click on **Connect** 
![Connect Maker channel to you account](https://raw.githubusercontent.com/berndverhofstadt/IboodDailyNotifier/master/images/ifttt_maker_connect.png)


Great you are connected to the IFTTT Maker Module!
You must be able to see you Key.



![Connect to Maker channel ](https://raw.githubusercontent.com/berndverhofstadt/IboodDailyNotifier/master/images/ifttt_maker_connected.PNG)



#### Make an IFTTT recipe

Now it's time to create a recipe.
Go to [https://ifttt.com/myrecipes/personal](https://ifttt.com/myrecipes/personal)

1. Click on "Create Recipe"
![Connect to Maker channel ](https://raw.githubusercontent.com/berndverhofstadt/IboodDailyNotifier/master/images/ifttt_recipe_create.png)
2. Click on "This"
![Select This](https://raw.githubusercontent.com/berndverhofstadt/IboodDailyNotifier/master/images/ifttt_recipe_ifthis.PNG)
3. Search for Maker and hit it!
![Search channel Maker](https://raw.githubusercontent.com/berndverhofstadt/IboodDailyNotifier/master/images/ifttt_recipe_ifthis_maker.png)
4. Click on "Receive a web request"
![Select receive a web request](https://raw.githubusercontent.com/berndverhofstadt/IboodDailyNotifier/master/images/ifttt_recipe_ifthis_maker_trigger.PNG)
5. Enter a name for you trigger
![Enter trigger eventname](https://raw.githubusercontent.com/berndverhofstadt/IboodDailyNotifier/master/images/ifttt_recipe_ifthis_maker_trigger_name.PNG)
6. Click on that.
![Select That](https://raw.githubusercontent.com/berndverhofstadt/IboodDailyNotifier/master/images/ifttt_recipe_thenthat.PNG)

Now you have to choose an channel where or how you want to recieve the notification.
When you are done Click on "Create Recipe"

#### Enter the settings in the app ####

When you've build the app. Open it.

It should look like this

![Ibood Notification center](https://raw.githubusercontent.com/berndverhofstadt/IboodDailyNotifier/master/images/ibood_notification_center.PNG)

Enter your eventname that you've chosen.
Your IFTTT key is dipslayed on [https://ifttt.com/maker](https://ifttt.com/maker)

#### Simple App Manual ####

- Select your country (Ibood is only available in a few countries! 
- Enter in the "Search For" textbox one keyword/line.
- Add you EventName and key as discriped above.
- Click on Check Ibood now to see if there is any product for. You'll receive a notification from IFTTT if there is one or more products found.

#### Check Ibood automaticly every day ####

- Open Task Scheduler on your computer and create new basic task.
- Enter the trigger frequency, day, hour,...
- Choose as action 'Start a program'
- Search for the `IboodDailyNotifier.exe` 
- Add the following argument: `-SilentNow "true"`



Have fun!
