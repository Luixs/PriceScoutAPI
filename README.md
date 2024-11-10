
# 📈 Price$coutAPI 

A microservice responsible for consulting and comparing product prices from multiples sources (such as e-commerce APIs and marketplaces) and providing a consolidated response to the user.


## 💻 Technologies

**Back-end:** ASP.NET Core

**Main Packages Installed**

* HttpClient: To consume external APIs.
* Serilog: For Structured logs, saved locally as the API has no integration with any DB.
* Polly: To implement resilience standards. In this case, i use 'retry'.
* xUnity: For unity tests to validate API.
* FakeItEasy: Mocking Library for creating fake objects in tests.

## 🚀 How' that work?

#### 🔄 **Current search engine**

**Search Process:**  

*`Change Currency`* ⟶ *`Define Country`* ⟶ *`Search Products On Marketplaces`* ⟶ *`Choose Best Option`* ⟶ *`Apply Filters`* ⟶ *`Return List`*



#### **Details:**
1. **💵 Change Currency**  
   Every search is started in the default currency (USD). If the user enters another currency, the API converts the values to the dollar (using another API) at the currency rate and, at the end of proccess, applies the reverse conversion to the requested currency.

2. **🌐 Define Country**  
   The country informed influences the choice and consultation of marketplaces. If any integrations fails, retry policies are applied to ensure the continuity of the process.

3. **⚙️ Search Products on MarketPlaces**  
   We carry out an independent search on various marketplaces to find the products

4. **🔎 Choose Best Option**  
   After the search, each product is mapped to the response model and goes through a system to choose the best option.

5. **📋 Apply Filters**  
   The API applies additional filters to refine the data and returns acurrence list to the user.


#### 🎯 **Inputs**

Only the **product name** is required.
```json
{
    "productName": "PlayStation 5"
}
```
 Other parameters are optional to refine the search:
- **💵 Minimum/Maximum Value:** Price limit for products.
- **🌍 Currency and Country of Sale:** Set the preferred currency and country for a specific search.
- **🔢 Max Products:** Control the number of results returned.

#### 📊 **Outputs**
> Our API returns not only the list of products! Here's what you get:
- **🛍️ Products:** List of products found.
- **📈 Analysis:**
  - Total products found.
  - Lowest and highest value.
  - Average prices.
  - **🏆 Best Option :** Calculated based on price, reviews and seller reputation.




## 🛠️ Running the Local Project

Clone the repository to your local machine. Then, install the dependencies and run the project

```bash
git clone https://github.com/Luixs/PriceScoutAPI      # --- Clone the repository
cd project_folder                                     # --- Access the main project folder
dotnet restore                                        # --- Install all dependencies
dotnet run                                            # --- Run the project
```
## References

 - [Polly](https://github.com/App-vNext/Polly) [Library]
 - [Serilog](https://serilog.net) [Library]
 - [xUnity.net](https://xunit.net) [Unity Tests]
  - [HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-8.0) [HTTPS Requests]
 - [FakeItEasy](https://fakeiteasy.github.io) [Mocking Library]
 - [Swagger](https://learn.microsoft.com/pt-br/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-8.0) [API Documentation]


## 🔗 Links
[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/luis-starlino/)

