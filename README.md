Part 1: Render a list from API call
    - Data API -  https://emplistapi-258220.appspot.com/
    - API returns list employees with the following format:
            {
              "name": {
                        "first": string,
                        "last": string,
                      },
              "jobTitle": string,
              "photoURL": string?
            }
    - The API accepts two parameters: pageSize, pageNumber (theres a limited amount of mock data, high 
      page numbers wont contain data)
    - Employees are required to have a first name, last name, and job title.
    - Not all employees have a photo
    - Sample list view - https://storage.googleapis.com/sampleemplistbucket/interviewsample.jpg
 Part 2: Implement an "Add Employee" button
    - Clicking the button should show a modal form containing all possible fields used to add an employee
    - The data provided doesn't actually need to save to the server. Clicking
      "save", however, should show a message that the employee was saved, then close the      
      modal. 
    - The data that would have been saved to the server should be the return value of the function
    - There are a limited amount of job title options
    - Required employee information should be validated before saving
 Part3:Add Sorting by Name ,jobTitle ,HasImage
 
 resource :
 Bulma - https://bulma.io/documentation/   Bulma is a free, open source framework to build responsive web interfaces. 
 X.pagedList  Library for easily paging throught any IEnumerable/iQueryqable in .NET and .net Core
 Newtonsoft.Json  Json.NET  high-performance JSON franework for .NET
