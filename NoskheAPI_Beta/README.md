# Documentation
## Introduction
This page is the key guide to android and desktop developers using Noskhe server as through their development. Hope you contribute to this and sharing bugs!
<br />
<br />
**Contents:**
* [Models](#models)
* [Desktop](#desktop)
* [Android](#android)
## Models
The models are divided into Input, Output:
### Input models
Input model is used to send a set of information as an object for the noskhe-server. POST and PUT methods need input. you should send an input model as a parameter for the functions (URLs) you use. you can find out an appropriate input model to the function according to http request reference to attach.

### Output models
Output model is used to interpret the output JSON data (return type) of a function. GET, POST, PUT methods have output. you can easily assign the response JSON data into the appropriate model. you can find out an appropriate input model to the function according to http request reference to attach.


## Clients
### Desktop
Request pattern: `http://server-ip:server-port/desktop-api/pharmacy/`
> Call an appropriate HTTP request method after the request pattern (as above). <br /> Example: `http://localhost:5000/desktop-api/pharmacy/get-database-status`
#### API key
use `desktop-api-key` key with the `OWQ21KJED0ASDWQE0POCXM30239J` value in request header.
> Note: If you don't include API keys in your project you will get *403 response* from server with *Invalid API Key* error.

We should add these lines of code below to the constructor of Repository class (a class or set of classes in which all the communication with the server are handled to make development easier). Therefore, this API key is applied for all server requests.
```javascript
HttpClientObj.BaseAddress = new Uri("IPAddress:port");            
HttpClientObj.DefaultRequestHeaders.Accept.Clear();
HttpClientObj.DefaultRequestHeaders.Add("desktop-api-key", "OWQ21KJED0ASDWQE0POCXM30239J");
HttpClientObj.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
```
#### Models
##### Input models
[Settle.cs](Models/Minimals/Input/Settle.cs)
```javascript
Settle
└──── Email < string >  // Pharmacy email address
└──── NumberOfOrders < int >  // Number of orders in which the pharmacy wants to be settled
└──── UPI < string >  // Unique Pharmacy Identifier
└──── Credit < decimal >  // Amount of money that a pharmacy has in our account
```
##### Output models
[Pharmacy.cs](Models/Minimals/Output/Pharmacy.cs)
```javascript
Pharmacy
└──── Name < string >
└──── UPI < string >  // Unique Pharmacy Identifier
└──── Email < string >
└──── Phone < string >
└──── ProfilePictureUrl < string >  // The url of pharmacy profile picture
└──── Address < string >  // Pharmacy address in Persian
└──── IsAvailableNow < boolean >  // Pharmacy service status
└──── Credit < decimal >  // The money that a pharmacy achieved from orders in our bank account
└──── ManagerName < string >  // Pharmacy owner name
```
[Score.cs](Models/Minimals/Output/Score.cs)
```javascript
Score
└──── UPI < string >  // Unique Pharmacy Identifier
└──── PharmacyName < string >
└──── CustomerSatisfaction < int >  // This is a factor that shows how a customer satisfied with the pharmacy service
└──── RankAmongPharmacies < int >  // This is a factor that shows the rank of a pharmacy among others
└──── PackingAverageTimeInSeconds < int >  // The average time spent through packing
```
[Settle.cs](Models/Minimals/Output/Settle.cs)
```javascript
Settle
└──── USI < string >  // Unique Settle Identifier
└──── CommissionCoefficient < double >  // Coefficient used to calculate commission
└──── NumberOfOrders < int >  // Number of orders in which the pharmacy wants to be settled
└──── Date < DateTime >  // The date in which the settle has been requested
└──── HasBeenSettled < bool >  // Specifies whether the settle has been paid to pharmacy or not
└──── Credit < decimal >  // The money that a pharmacy achieved from orders in our bank account
```
[Descriptive.cs](Models/Response/Descriptive.cs)
```javascript
Descriptive
└──── Success < boolean >  // Specifies whether the server response was true or not. in case of true, ther is no 'Message'
└──── Message < string >  // Describes why a response failed if there is one
```
#### HTTP request methods reference
* `get-database-status`
   * **Method:** GET
   * **Parameter(s):** none
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** DATABASE_FAILURE
   * **Description:** Checks whether the server can receive any data from database file or not.

* `get-server-status`
   * **Method:** GET
   * **Parameter(s):** none
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** none
   * **Description:** Checks whether the server communicates properly to it's clients.

* `get-details`
   * **Method:** GET
   * **Parameter(s):** 'upi' string
   * **Output(s):** single 'Pharmacy' model, single 'Descriptive' model
   * **Error(s):** NO_PHARMACIES_MATCHED_THE_UPI, DATABASE_FAILURE
   * **Description:** Returns pharmacy details which can be used to show in profile.
   
* `get-upi`
   * **Method:** GET
   * **Parameter(s):** 'email' string
   * **Output(s):** single 'UPI' value
   * **Error(s):** NO_PHARMACIES_MATCHED_THE_EMAIL, DATABASE_FAILURE
   * **Description:** Returns 'UPI' that has that input Email address
   
* `get-orders`
   * **Method:** GET
   * **Parameter(s):** 'upi' string
   * **Output(s):** multiple 'Orders' models, single 'Descriptive' model
   * **Error(s):** BAD_START_TIME_FORMAT, BAD_END_TIME_FORMAT, START_TIME_IS_GREATER_THAN_END_TIME, NO_RESPONSES_MATCHED_THE_UPI, DATABASE_FAILURE
   * **Description:** Returns a list of orders which are linked to the pharmacy.

* `get-score`
   * **Method:** GET
   * **Parameter(s):** 'upi' string
   * **Output(s):** single 'Score' model, single 'Descriptive' model
   * **Error(s):** NO_SCORES_MATCHED_THE_UPI, DATABASE_FAILURE
   * **Description:** Returns pharmacy score details.

* `get-settles`
   * **Method:** GET
   * **Parameter(s):** 'upi' string
   * **Output(s):** multiple 'Settle' models, single 'Descriptive' model
   * **Error(s):** NO_SETTLES_MATCHED_THE_UPI, DATABASE_FAILURE
   * **Description:** Gets a list of settles in which the pharmacy has submitted earlier.

* `set-settle`
   * **Method:** POST
   * **Parameter(s):** single 'Settle' model
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** NO_PHARMACIES_MATCHED_THE_UPI, DATABASE_FAILURE
   * **Description:** Sets a settle for the pharmacy which has requested.

* `get-top-five`
   * **Method:** GET
   * **Parameter(s):** 'upi' string
   * **Output(s):** multiple 'Score' models, single 'Descriptive' model
   * **Error(s):** DATABASE_FAILURE
   * **Description:** Returns the top five pharmacy based on their score given during their activity.

* `authenticate`
   * **Method:** POST
   * **Parameter(s):** credential string[] (credential[0]: email, credential[1]: password)
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** VERIFICATION_FAILED, DATABASE_FAILURE
   * **Description:** Checks whether the pharmacy client entered the right password or not. Therefore, the application can redirect the client to pharmacy main page.

* `change-status`
   * **Method:** PUT
   * **Parameter(s):** 'upi' string
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** NO_PHARMACIES_MATCHED_THE_UPI, DATABASE_FAILURE
   * **Description:** Used to change the status of the pharmacy to ON/OFF. Basically, ON means it will receive new orders from customers and OFF means the pharamcy service is unavailabe.

* `get-weekly-number-of-orders`
   * **Method:** GET
   * **Parameter(s):** 'upi' string
   * **Output(s):** multiple 'float' values, single 'Descriptive' model
   * **Error(s):** NO_PHARMACIES_MATCHED_THE_UPI, DATA_IS_NOT_AVAILABE, DATABASE_FAILURE
   * **Description:** Returns the daily number of orders in the recent week which a pharmacy has. It is an array of 8 floats. The first 7 items are the number of orders of week days. The last item is the average number of oreders of week.

* `get-weekly-packing-average-time`
   * **Method:** GET
   * **Parameter(s):** 'upi' string
   * **Output(s):** multiple 'float' values, single 'Descriptive' model
   * **Error(s):** NO_PHARMACIES_MATCHED_THE_UPI, DATA_IS_NOT_AVAILABE, DATABASE_FAILURE
   * **Description:** Returns packing average time during the recent week, which is an array of 8 floats. The first 7 items are the time for the week days. The last item is for total packing average time of week.

### Android
Request pattern: `http://server-ip:server-port/android-api/customer/`
> Call an appropriate HTTP request method after the request pattern (as above). <br /> Example: `http://localhost:5000/desktop-api/pharmacy/get-database-status/get-details?email="example@email.com"`
#### API key
use `android-api-key` key with the `J76Y2U3UO8IDUWQDHWQKIDP2UIDS` value in request header.
> Note: If you don't include API keys in your project you will get *403 response* from server with *Invalid API Key* error.
#### Models
##### Input models
[Customer.cs](Models/Minimals/Input/Customer.cs)
```javascript
Customer
└──── FirstName < string >
└──── LastName < string >
└──── Gender < Gender >  // Custome type, see  Gender in Enum section for more details of this type
└──── Birthday < DateTime>
└──── Email < string >
└──── Password < string >  // Raw password should be set, server generates Base256 hash to save into database
└──── Phone < string >
└──── ProfilePictureUrl < string >  // The url of customer profile picture
```
[ShoppingCart.cs](Models/Minimals/Input/ShoppingCart.cs)
```javascript
Shopping Cart
└──── Email < string >  // 
└──── BrandPreference < BrandType >  // Custome type, see  BrandType in Enum section for more detail
└──── HasPregnancy < boolean >
└──── HasOtherDiseases < boolean >
└──── Description < string >  // Required when the customer checks the 'HasOtherDiseases' item
└──── PictureUrl_1 < string >
└──── PictureUrl_2 < string >
└──── PictureUrl_3 < string >
└──── AddressLongitude < double >  // Longitude of position which is taken from map
└──── AddressLatitude < double >  // Longitude of position which is taken from map
└──── Address < string >  // Shopping Cart address in Persian
└──── MedicineIds < int[] >  // List of medicine id
└──── CosmeticIds < int[] >  // List of Cosmetic id
└──── HasBeenRequested < boolean >  // True if the customer wants his/her shopping cart to be processed by pharmacy
```
##### Output models
[Cosmetic.cs](Models/Minimals/Output/Cosmetic.cs)
```javascript
Cosmetic
└──── Name < string >
└──── Price < decimal >
└──── ProductPictureUrl < string >  // Cosmetic picture URL
```
[Medicine.cs](Models/Minimals/Output/Medicine.cs)
```javascript
Medicine
└──── Name < string >
└──── Price < decimal >
└──── ProductPictureUrl < string >  // Medicine picture URL
```
[Customer.cs](Models/Minimals/Output/Customer.cs)
```javascript
Customer
└──── FirstName < string >  // 
└──── LastName < string >  // 
└──── Gender < Gender >  // Custome type, see  Gender in Enum section for more detail
└──── Birthday < DateTime >
└──── Email < string >
└──── Phone < string >
└──── ProfilePictureUrl < string >  // The url of customer profile picture
```
[Order.cs](Models/Minimals/Output/Order.cs)
```javascript
Order
└──── UOI < string >  // Unique Order Identifier
└──── DateTime Date < >  // The date that the order has been requested to be handled
└──── TotalPriceWithoutShippingCost < decimal >  // Includes the price of order (cosmetics, medicines, items of prescription)
└──── TotalPrice < decimal >  // Includes price of order (cosmetics, medicines, items of prescription) plus shipping cost
└──── HasPrescription < bool >  // Specifies whether the customer has added a prescription or not
└──── HasBeenAcceptedByCustomer < bool >  // Specifies whether the customer has accepted the total price after shipping cost being added to invoice
└──── HasBeenPaid < bool >  // Specifies whether the order has been paid by the customer or not
└──── HasBeenDeliveredToCustomer < bool >
└──── CourierName < string >
└──── PharmacyName < string >
└──── PharmacyAddress < string >  // Pharmacy address in Persian
└──── Address < string >  // Shopping Cart address in Persian
└──── Email < string >  // Owner (customer) email address
└──── Cosmetics < Dictionary<string, string[]> >  // Key: Name, Value[]: Quantity, Price
└──── Medicines < Dictionary<string, string[]> >  // Key: Name, Value[]: Quantity, Price
└──── BrandPreference < BrandType >  // Custome type, see  BrandType in Enum section for more detail
└──── HasPregnancy < bool >
└──── HasOtherDiseases < bool >
└──── Description < string >  // Required when the customer checks the 'HasOtherDiseases' item
└──── HasBeenAcceptedByPharmacy < bool >  // Specifies whether the order has been accepted by the pharmacy or not
└──── PictureUrl_1 < string >
└──── PictureUrl_2 < string >
└──── PictureUrl_3 < string >
└──── PrescriptionItems < Dictionary<string, string[]> >  // The items where the pharmacy take from prescription pictures. Key: Name, Value[]: Quantity, Price
└──── PackingTime < int >  // The time spent for packing a customer order
```
[ShoppingCart.cs](Models/Minimals/Output/ShoppingCart.cs)
```javascript
Shopping Cart
└──── ShoppingCartId < int >  // Id of shopping cart
└──── USCI < string >  // Unique Shopping Cart Identifier
└──── DateTime Date < >  // The date that shopping cart has been created
└──── Address < string >  // Shopping Cart address in Persian
└──── Email < string >  // Owner (customer) email address
└──── Cosmetics < Dictionary<string, string[]> >  // Key: Name, Value[]: Quantity, Price
└──── Medicines < Dictionary<string, string[]> >  // Key: Name, Value[]: Quantity, Price
└──── BrandPreference < BrandType >  // Custome type, see  BrandType in Enum section for more detail
└──── HasPregnancy < bool >
└──── HasOtherDiseases < bool >
└──── Description < string >  // Required when the customer checks the 'HasOtherDiseases' item
└──── PictureUrl_1 < string >
└──── PictureUrl_2 < string >
└──── PictureUrl_3 < string >
└──── TotalPriceWithoutPrescription < decimal >  // Includes the price of order (cosmetics and medicines, but not items of prescription)
└──── HasBeenRequested < bool >  // True if the customer wants his/her shopping cart to be processed by pharmacy
```
[Descriptive.cs](Models/Response/Descriptive.cs)
```javascript
Descriptive
└──── Success < boolean >  // Specifies whether the server response was true or not. in case of true, ther is no 'Message'
└──── Message < string >  // Describes why a response failed if there is one
```

#### HTTP request methods reference
* `get-details`
   * **Method:** GET
   * **Parameter(s):** 'email' string
   * **Output(s):** single 'Customer' model, single 'Descriptive' model
   * **Error(s):** NO_CUSTOMERS_MATCHED_THE_EMAIL, DATABASE_FAILURE
   * **Description:** Returns customer information which can be used in profile.

* `get-shopping-carts`
   * **Method:** GET
   * **Parameter(s):** 'email' string
   * **Output(s):** multiple 'ShoppingCart' models, single 'Descriptive' model
   * **Error(s):** NO_SHOPPING_CARTS_MATCHED_THE_EMAIL, DATABASE_FAILURE
   * **Description:** Returns customer shopping carts which can be used to show in application.

* `get-orders`
   * **Method:** GET
   * **Parameter(s):** 'email' string
   * **Output(s):** multiple 'Order' models, single 'Descriptive' model
   * **Error(s):** NO_ORDERS_MATCHED_THE_EMAIL, DATABASE_FAILURE
   * **Description:** Returns customer orders which can be used to show in application.

* `get-cosmetics`
   * **Method:** GET
   * **Parameter(s):** none
   * **Output(s):** multiple 'Cosmetic' models, single 'Descriptive' model
   * **Error(s):** NO_COSMETICS_AVAILABE, DATABASE_FAILURE
   * **Description:** Returns all cosmetics stored in database which can be used to setup availabe cosmetic list to buy.

* `get-cosmetics`
   * **Method:** GET
   * **Parameter(s):** 'usci' string
   * **Output(s):** multiple 'Cosmetic' models, single 'Descriptive' model
   * **Error(s):** NO_COSMETICS_MATCHED_THE_USCI, DATABASE_FAILURE
   * **Description:** Returns a list of cosmetics of a shopping cart which is owned by customer.

* `get-list-of-cosmetic-ids`
   * **Method:** GET
   * **Parameter(s):** 'usci' string
   * **Output(s):** multiple 'int' values, single 'Descriptive' model
   * **Error(s):** NO_COSMETICS_MATCHED_THE_USCI, DATABASE_FAILURE
   * **Description:** Returns a list of cosmetic IDs in which customer has added to his/her own shopping cart.

* `get-medicines`
   * **Method:** GET
   * **Parameter(s):** none
   * **Output(s):** multiple 'Modicine' models, single 'Descriptive' model
   * **Error(s):** NO_MEDICINES_AVAILABE, DATABASE_FAILURE
   * **Description:** Returns all medicines stored in database which can be used to setup availabe medicine list to buy.

* `get-medicines`
   * **Method:** GET
   * **Parameter(s):** 'usci' string
   * **Output(s):** multiple 'Modicine' models, single 'Descriptive' model
   * **Error(s):** NO_MEDICINES_MATCHED_THE_USCI, DATABASE_FAILURE
   * **Description:** Returns a list of medicines of a shopping cart which is owned by customer.

* `get-list-of-medicine-ids`
   * **Method:** GET
   * **Parameter(s):** 'usci' string
   * **Output(s):** multiple 'int' values, single 'Descriptive' model
   * **Error(s):** NO_MEDICINES_MATCHED_THE_USCI, DATABASE_FAILURE
   * **Description:** Returns a list of medicine IDs in which customer has added to his/her own shopping cart.

* `authenticate`
   * **Method:** POST
   * **Parameter(s):** 'credential' string[] (credential[0]: email, credential[1]: password)
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** VERIFICATION_FAILED, DATABASE_FAILURE
   * **Description:** Checks whether the customer entered right login credentials or not in order to take the customer to his/her own page.

* `authenticate`
   * **Method:** POST
   * **Parameter(s):** 'phone' string
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** NO_CUSTOMERS_MATCHED_THE_PHONE, DATABASE_FAILURE
   * **Description:** Checks whether the customer entered right phone number or not in order to send SMS to his/her mobile. Also, this generates a verification in the database to check later.

* `send-sms-authentication-code`
   * **Method:** POST
   * **Parameter(s):** 'phone' string
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** , DATABASE_FAILURE
   * **Description:** Sends SMS to the customer phone number.

* `verify-sms-authentication-code`
   * **Method:** POST
   * **Parameter(s):** 'pair' string[] (pair[0]: phone, pair[1]: verification code)
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** VERIFICATION_EXPIRED, VERIFICATION_FAILED, DATABASE_FAILURE
   * **Description:** Checks whether customer entered the right verification code or not.

* `add-new`
   * **Method:** POST
   * **Parameter(s):** single 'Customer' model
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** DUPLICATE_CUSTOMER, DATABASE_FAILURE
   * **Description:** Adds a new customer to the application.

* `edit-existing`
   * **Method:** PUT
   * **Parameter(s):** single 'Customer' model
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** NO_CUSTOMERS_MATCHED_THE_EMAIL, DATABASE_FAILURE
   * **Description:** Edits detais of an existing customer.

* `add-new-shopping-cart`
   * **Method:** POST
   * **Parameter(s):** single 'ShoppingCart' model
   * **Output(s):** single 'Descriptive' model
   * **Error(s):** NO_CUSTOMERS_MATCHED_THE_EMAIL, DATABASE_FAILURE
   * **Description:** Adds a new shopping cart for a customer.
