# Housing Code Challenge : Short Term Rental Data Sharing System

This project is for the purposes of the Ministry of Housing Code Challenge where an Open Geo-spatial Consortium (OGC) compliant web map
application for managing property owner requests for short-term rental status.


# Description

This application provides three high-level features:
# Application Features

This application provides a comprehensive set of features to enhance user authentication, authorization, and geospatial mapping capabilities.

## Authentication:

Users can sign up and log in securely using a local username/password scheme. Upon successful authentication, the application issues a JWT token, which serves as a secure and efficient means of authentication for all subsequent REST API requests.

## Authorization:

The application implements a role-based access control system to grant users access to specific data based on their assigned roles. This ensures that users interact only with information relevant to their permissions, maintaining a secure and tailored user experience.

## Geospatial Mapping:

### Address Capture and Validation:

The application captures street addresses from users and validates them using the [BC Address GeoCoder](https://digital.gov.bc.ca/bcgov-common-components/bc-address-geocoder/). This external service ensures the accuracy and validity of provided addresses. The application accepts street addresses if the search score is greater than 90 and the match precision is CIVIC_NUMBER or BLOCK for rural areas.

### GeoCoder Integration:

Upon validation, the GeoCoder returns latitude and longitude coordinates associated with the captured street addresses. These coordinates are utilized by the application to enhance the accuracy and precision of the geospatial mapping functionality.

### Dynamic Map Display:

Users can explore a dynamic geospatial map within the application, providing a visually intuitive representation of the geographical locations associated with the captured data. The map is powered by the validated coordinates, ensuring real-world accuracy and up-to-date spatial information.

### CI/CD Pipeline

Utilized Helm Chart and Github Actions for build and deployment to Openshift

## Conclusion:

Together, these features create a robust and user-friendly environment, combining secure authentication, fine-grained authorization, and a visually informative geospatial mapping experience.


## Getting Started

### Development Tools

* Visual Studio 2022
* Visual Studio Code
* Docker
* Git

### Development Instructions
* Clone the [repository](https://github.com/BCDevExchange-CodeChallenge/cc-swu-HOUS-str-blue.git).
* Server
  - Open server/AdvSol.sln in Visual Studio
  - Hit F5 to run
* Frontend
  - Open frontend folder in VS Code
  - Run
    ```
    npm run dev
    ```
* 

### Build and Deployment Instructions
* Clone the [repository](https://github.com/BCDevExchange-CodeChallenge/cc-swu-HOUS-str-blue.git).

* Open a command prompt and navigate to the `cc-swu-HOUS-str-blue` root folder
* Run 
  ```
  docker-compose up
  ```
* After successful build, open Docker Desktop and make sure the three containers (DB, Server and Frontend Web App) are running
* Open a web browser and type in the URL below

  ```
  http://127.0.0.1:5002/
  ```

### Environment Variables

The application will run without modifications to the settings files. However, these may be changed in the `/docker-compose.yml` file if necessary.  

Note: If changes are made to the configuration, the application will need to be deployed again.

## Application Architecture and Technical Design Notes

* PostGIS/PostgreSQL database provides the persistence layer with support for geographic objects
* Single Page Application architecture
* SOLID design principals
* Code-first approach for database creation and initial data seeding using Entity Framework
* Twelve-Factor App methodology
* .NET 7 provides back-end functionality
* Angular front-end - responsive design for mobile and desktop functionality
* Docker containers and one-step build and deployment
* Automated UI testing with 

### Project Structure - Key Folders 
```
|
├───frontend (Front-end Application)
│   ├───src 
│   │   ├───app
│   │   │   ├───common
│   │   │   ├───features
│   │   │   ├───assets
├───postman ( API Testing )
├───server (Back-end Application)
│   ├───AdvSol 
│   │   ├───Authentication
│   │   ├───Authorization
│   │   ├───Controllers 
│   │   ├───Data  
│   │   ├───Migrations
│   │   ├───Services
│   ├───AdvSol.Tests (Unit Tests - Back-end)
├───UITest (Unit Tests - Front-end)
│   ├───TestFramework
│   ├───XUnitTests
```

## Testing

A selection of automated testing is provided including front and back-end unit testing, integration tests and a test framework.

### Unit Tests

The solution includes unit tests for a selection of both the front-end and back-end.  These are located in  [UITest](./UITest) and [AdvSol.Test](./server/AdvSol.Tests) respectively. 

Unit tests are intended to be triggered automatically in a fully developed build and deployment pipeline. For the purposes of the code challenge,  unit tests may also be manually run from the terminal and Visual Studio. 

### UI Tests
The UI tests are written with a custom framework based on the Page Object Model design pattern and using Selenium Webdriver. 

Requirements:
.Net 7 SDK

Execution instructions to build and run the tests:
1. From the Command window, cd to the "UITest" directory
2. Run "dotnet build"
3. Run "dotnet test --filter TestUserRegisterAndSignInScenario"

#### Known Issues with UITests
* As per requirements, there is a limit of 20 applicants. Running the test automation will create a unique user each time the test is run. Therefore there is a limitation of 20 test runs maximum. Running the tests more than 20 times will generate an error in the test logs.

## Usage Notes

The following notes provide instructions and tips for using the application and a summary of the user stories.

### User Stories

#### User Story #1 – Register as a "Permit Applicant"

1. Click the "Sign Up" button to access the registration form.
2. Complete the registration form with the required information.
3. Submit the form for processing.
4. The application will utilize [BC Address GeoCoder](https://digital.gov.bc.ca/bcgov-common-components/bc-address-geocoder/) to validate the provided street address. The validation criteria include a search score greater than 90 and a match precision of CIVIC_NUMBER or BLOCK.
5. Validate the phone number format to be in the pattern (xxx) xxx-xxxx.
6. Duplicate records with identical last names or usernames will not be accepted.
7. Once the total number of applicants reaches the maximum limit of 20, the application will no longer accept new applicants.
8. If the form entry meets all the validation criteria, it will be accepted. Otherwise, the application will provide error messages to indicate validation failure.
9. The application securely stores the hash value of the password.


#### User Story #2 – Log Into the Application

1. Enter the username and password, then submit.
2. The application calculates the hash value of the password and validates it against the corresponding record in the database identified by the entered username.
3. Upon successful credential verification, the user will be directed to the application list page.
4. Note that two admin users are pre-seeded with the following credentials:

   - **ADMIN1**
     - **Password:** test
   - **ADMIN2**
     - **Password:** test

    - Username and password are case sensitive

#### User Story #3 – Create an STR Permit Application (Applicant Only)

1. Click the "+" icon to add an STR permit application.
2. Fill out the form and submit.
3. The application will validate the address using [BC Address GeoCoder](https://digital.gov.bc.ca/bcgov-common-components/bc-address-geocoder/), employing the same criteria as the registration page.
4. The application sets Pending to Compliance status upon creation.
5. Once the total number of applications reaches the maximum limit of 5 for a given applicant, the application will no longer accept new application.
6. An admin user is restricted from creating applications.
7. Notifications are sent to all admins through the application. On the admin's screen, in the top-right corner, a notification icon will display the number of created applications. Clicking the icon will reveal the application details. The application periodically polls for notifications. The functionality to dismiss read notifications has not been implemented.

#### User Story #4 – Read an STR Permit Application (Applicant/Admin)

1. Upon opening the list page, all applications belonging to the user (applicant) are displayed. If the user is an admin, all applications are displayed.

#### User Story #5 – View a Map of STR Permit Applications (Applicant/Admin)

1. A map is displayed above the application list on the list page.
2. All the locations of the STR permit applications are represented as pins on the map.
3. When clicking a pin, the corresponding address is displayed on the map.
4. Clicking on a link within a row in the application list moves the map to center on the corresponding location.
5. The color of the pin on the map depends on the compliance status.
   - Pending: Yellow
   - Compliant: Green
   - Non-compliant: Red

#### User Story #6 / #7 – Update an STR Permit Application (Applicant/Admin)

1. Clicking the pen icon on the right of each row displays the application edit form.
2. Fill out the form and submit. The same validation logic applies.
3. If the user is an applicant and attempts to change the compliance status, an error message will be displayed upon submission.
4. If the user is an admin, the compliance status change will be saved.
5. The application will log update timestamp, field changed, new field value

#### User Story #8 - Filter the List of STR Applications by Geometry (Applicant/Admin)

1. Click the map and move mouse to start drawing a circle on the map.
2. Cick to finish drawing the circle.
3. The application list shows only the applications that are located inside of the circle.

#### User Story #9 – Display an Application's Logs

1. Navigate to the application detail and click the history button
2. The application displays the history of the application

#### User Story #10 – Log Out of the Application

1. On the right corner, locate the exit icon.
2. Clicking the icon logs you out of the application.

## Known Issues
Due to the time constraints of this exercise, the following issues were not addressed:
* Field-level validation for input forms is not implemented in the frontend, however the server comprehensively validates all submitted data.
* Accessibility features were neither implemented nor tested






