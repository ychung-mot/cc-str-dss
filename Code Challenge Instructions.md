# Code Challenge Notice, Instructions & Rules
Re: Competition "Short Term Rental Data Sharing System" (the “RFP”)

Government Contact: ian.fonberg@gov.bc.ca 

This notice is dated January 15, 2024 (the “Notice Date”).

Congratulations - you are a Shortlisted Proponent eligible to participate in the Code Challenge.

## Rules and Instructions
Please be advised of the following rules and instructions:
1. These code challenge rules and instructions apply only to Shortlisted Proponents and are part of the RFP.
2. Shortlisted Proponents will have three (3) Business Days from the Notice Date to complete the Code Challenge. The deadline to complete the code challenge in accordance with these rules is 9:00 a.m. Pacific Time on Thursday, January 18, 2024 (the “Deadline”).
3. The Shortlisted Proponent’s Code Challenge submission Deliverable (defined below) must be received by the Province (as provided for by these instructions) and be deposited and located in the applicable Repository before the Deadline, failing which such submission will not be eligible for evaluation and the associated Shortlisted Proponent Proposal will receive no further consideration and such Shortlisted Proponent will be eliminated from the RFP competition.
4. Only the Proponent Resources that were put forward in a Shortlisted Proponent’s RFP Proposal are eligible to participate in the Code Challenge.
5. The Shortlisted Proponent Resources will be sent invites via GitHub to join this private repository. 
6. As of the Notice Date, the Code Challenge issue has been created in this private repository under the "BCDevExchange-CodeChallenge" organization.
7. Shortlisted Proponents may direct clarifying questions by creating an issue in the applicable GitHub Repository. Any such questions must be received by the Government Contact before 4:00 pm Pacific Time on Tuesday, January 16, 2024 (the "Code Challenge Questions Deadline").
8. The Province reserves the right to amend this notice before or after the Closing
Time, including changing the Deadline or the Code Challenge Questions Deadline upon notice to all Shortlisted Proponents.
9. The Shortlisted Proponent must complete all of the following tasks and the Deliverable and as such they must be deposited and received in the applicable Repository by the Province in the form specified by this notice before the
Deadline:
* Complete all code changes required to complete the code challenge (the "Deliverable"); and
* Attach an Apache License 2.0 to the Deliverable.
10. The rules and instructions set forth in this notice are in addition to any rules, terms and conditions set forth elsewhere in the RFP.

# Code Challenge (Short Term Rental Data Sharing System)

## Code Challenge Instructions

# Deliverable

### Introduction

This code challenge asks you to build a containerized geospatial web application to manage property owner requests for Short-Term Rental (STR) status.  You must create a mapping application that allows users to view spatial data.  You must also create a database to manage the underlying STR information.

The application must allow the user to perform these functions:

* Authenticate using a local username/password scheme;
* Grant the user access to the application's data based on the user's role; and
* View a geospatial map that displays the geographical location of the application's data

The application has two different types of "Users":

* **Owner Applicants**: users authenticated in the system with an "Applicant" role; and
* **STR Admins**: users authenticated in the system with an "Admin" role 

The database shall manage the applications's "STR Applications".  

Each "STR Application" item must include the following fields:
* Property owner's name
* Property's street address
* Zoning type (limited to "Residential", "Commercial" or "Mixed Use" only)
* Square footage
* STR affiliate (e.g., AirBNB, VRBO)
* Property owner's primary residence
* Compliance status (limited to "Pending", "Compliant" or "Non-compliant")
* Creation timestamp

### Technical Requirements

For the database, participating teams are required to use some form of SQL database (e.g., PostgreSQL or equivalent). 

Participating teams are not limited to a certain technology stack or any specific technologies.  We do encourage everyone to use a stack that is commonly used in developing modern web applications.  For instance, a good choice of framework would be Angular, React or Vue.

You can use the mapping technology of your choice, but it will have to support valid street addresses within the province of British Columbia.

You can use the containerization technology of your choice (e.g., Docker).  Please submit a solution that the evaluation team can build and deploy locally with a single CLI command (e.g., "docker compose up").

## User Stories
There are 10 user stories in all. They may be completed in any order. 

#### User Story #1 – Register as a "Permit Applicant"
On first instance, I want to be able to register as an STR applicant

**Given** that I am accessing the application for the first time<br/>
**And** I do not have an STR applicant account<br/>
**And** I have navigated to the "sign up" page<br/>
**When** I enter a unique username and password<br/>
**And** I complete the remaining required fields (see Implementation Notes below)<br/>
**And** I activate the "Sign Up" UI control<br/> 
**Then** a new "applicant" account is created in the database<br/>
**And** I am redirected to the "login" page<br/>

**Implementation Notes**

Each "applicant" account must include the following fields:
* Applicant's last name
* Applicant's street address
* Applicant's mobile phone number
* Creation timestamp 

For ease of implementation, we will assume that none of the applicants share the same last name.

For ease of implementation, we will assume that there are a maximum of 20 applicants in total.

#### User Story #2 – Log Into the Application
As a application user, I want to be able to log in as either an applicant or an admin

**Given** that I am registered with the application as either an applicant or an admin<br/>
**And** I am on the login page<br/>
**When** I enter my credentials (i.e., username and password)<br/>
**Then** I am logged in<br/>
**And** I am redirected to the "list of STR applications" page.<br/>

**Implementation Notes**

A list of admin users can simply be created and seeded into the application.

#### User Story #3 – Create an STR Permit Application (Applicant Only)
As a applicant, I want to be able to create an STR application.

**Given** that I am registered with the application as an STR applicant<br/>
**And** I am logged in to the application<br/>
**And** I have navigated to the "create an STR request" page<br/>
**When** I enter the required information and press submit<br/> 
**Then** a new STR application is saved and added to the list of STR applications<br/>
**And** I am redirected to the "list of STR applications" page<br/>
**And** All admins are notified that a new application has been created and needs to be reviewed for compliance.<br/>

**Implementation Notes**

Each STR application must include the following fields:
* Property owner's name (autopopulated)
* Property's street address
* Zoning type (limited to "Residential", "Commercial" or "Mixed Use" only)
* Square footage
* STR affiliate (e.g., AirBNB, VRBO)
* Property owner's primary residence
* Compliance status (limited to "Pending", "Compliant" or "Non-compliant")
* Creation timestamp

The maximum number of permits allowed for a given applicant is 5.

For ease of implementation, applicants can only apply for STR permits for properties that they own.

For ease of implementation, assume that any given property has one and only one property owner.

Admin users CANNOT create permit applications.

The only allowable zoning type values are "Residential", "Commercial" and "Mixed Use."

The only allowable compliance status values are "Pending", "Compliant" and "Non-compliant." 

#### User Story #4 – Read an STR Permit Application (Applicant/Admin)

**Use Case #1 - Applicant**

As a applicant, I want to be able to read an STR application that I have created.

**Given** that I am registered with the application as an applicant<br/>
**And** I am logged in to the application<br/>
**And** I navigate to the "list of STR applications" page<br/>
**When** I select a specific application from the list<br/>
**Then** the underlying map pans to the application's street address<br/>  
**And** the application's street address is highlighted<br/>   
**And** I am presented with a detailed view of the STR application<br/>

**Implementation Notes**

Each record in the "list of permit applications" must include the following fields:
* Property's street address
* STR affiliate
* Compliance status

An application's detailed view must display the following fields:
* Property's street address
* Zoning type 
* Square footage
* STR affiliate 
* Property owner's primary residence
* Compliance status 

The maximum number of permits allowed in an applicant's list is 5.

**Use Case #2 - Admin**

As a admin, I want to be able to read any STR application created by any of the application's applicants. 

This use case is identical to Use Case #1, with the following additional functions.  Admins are presented a list of STR applications that contains all of the STR permits created by all of the application's STR applicants.  Additionally, for admins each record in the "list of STR applications" and each application's detailed view must also include the applicant's last name.

The maximum number of permits allowed in the list is 100 (a maximum of 20 applicants times a maximum of 5 permits per applicant).

#### User Story #5 – View a map of STR Permit Applications (Applicant/Admin)

**Use Case #1 - Applicant**
As a applicant or admin, I want to be able to view the property addresses for my STR permit applications.

**Given** that I am registered with the application as an applicant<br/>
**And** I am logged in to the application<br/>
**When** I have navigated to the STR permit applications map page<br/> 
**Then** each of the property addresses for my STR permit applications are displayed on the map.<br/>
**And** I can pan the map.<br/>
**And** I can zoom in and out on the map.<br/>

**Use Case #2 - Admin**
As an admin, I want to be able to view the property addresses for all STR permit applications.

**Given** that I am registered with the application as an admin<br/>
**And** I am logged in to the application<br/>
**When** I have navigated to the STR permit applications map page<br/> 
**Then** each of the property addresses for all STR permit applications are displayed on the map.<br/>
**And** I can pan the map.<br/>
**And** I can zoom in and out on the map.<br/>

**Implementation Notes**

On the map, STR properties shall be color-coded as follows by compliance status:
* Pending - Yellow
* Compliant - Green
* Non-compliant - Red

If desired, the map page and "list of STR applications" page can be combined into a single page.

#### User Story #6 – Update an STR Permit Application (Applicant/Admin)
As a applicant or admin, I want to be able to edit an STR permit application.

**Given** that I am registered with the application as an applicant or admin<br/>
**And** I am logged in to the application<br/>
**And** I have navigated to the "list of STR applications" page<br/>
**And** I have selected an STR permit application from the "list of STR applications" page<br/>
**When** I activate the "Edit STR Application" UI control<br/> 
**Then** I am redirected to a view that allows me to edit the select STR application and save the changes.<br/>

**Implementation Notes**

Applicants/Admins must be able to edit the following fields:

* Property's street address
* Zoning type 
* Square footage
* STR affiliate 
* Property owner's primary residence

For each update operation, the following information must be logged:

* Update timestamp
* Field changed
* New field value

#### User Story #7 – Approve/Reject a Permit Application (Admin Only)
As a admin, I want to be able to review an STR application and update its compliance status.

**Given** that I am registered with the application as an admin<br/>
**And** I am logged in to the application<br/>
**And** I have navigated to the "list of STR applications" page<br/>
**And** I have selected a permit application from the "list of STR applications" page<br/>
**And** I activate the "Edit STR Application" UI control and am redirected to the Edit View<br/>
**When** I change the value of the application's "compliance status" field and save the change<br/> 
**Then** The value of the "compliance status" field is updated.<br/>
**And** The applicant is notified of the status change through the application.<br/>

**Implementation Notes**

How you implement the applicant notification is up to you, but make sure that you notify the applicant by way of the application.  Specifically, you do NOT have to notify the application by email.

#### User Story #8 - Filter the List of STR Applications by Geometry (Applicant/Admin)
As an applicant or admin, I want to be able to access the list of STR applications limited to a specific geometry.

**Given** that I am registered with the application as an applicant or admin<br/>
**And** I am logged in to the application<br/>
**And** I have navigated to the "list of permit applications" page<br/>
**When** I draw a geometry on the map (e.g., a circle or polygon)<br/> 
**Then** the list of STR applications is restricted to those properties that are located within the given geometry.<br/>

**Implementation Notes**

Your solution is required to support at least one type of geometry (e.g., a circle, a polygon).

#### User Story #9 – Display an Application's Logs
As an applicant or admin, I want to be able to view an STR application's logs.  

**Given** that I am registered with the application as a applicant or admin<br/>
**And** I am logged into the application<br/>
**And** I have navigated to an STR application's detailed view page<br/>
**When** I activate the page's "Application Log" UI control<br/>
**Then** I am presented with a log of the application's creation and all changes to the application record<br/>

**Implementation Notes**

Each STR application log item must include: 

* Item type (e.g. "Record Created", "Record Updated")
* Item timestamp
* For updates, the field that was changed and the updated field value

A single update item must accommodate bulk updates (that is, updates to multiple fields).

#### User Story #10 – Log Out of the Application
As an applicant or admin, I want to be able to log out of the application.

**Given** that I am registered with the application as a applicant or admin<br/>
**And** I am logged into the application<br/>
**When** I activate the "Logout" UI control<br/>
**Then** I am logged out of the system<br/>
**And** I am redirected to the login page<br/>

## Submission Requirements

1. Include all artifacts that are required to build and deploy the solution including code, Dockerfiles, other configuration files, etc.
2. Include all artifacts that would typically be included in the definition of done including demonstration of testing (full test suites are **not** required, but only a demonstration of how your team would test your solution).
3. Include documentation as to how your application works, to include documenation on how you satisfied the various user stories.
4. Include a README file with instructions for building and deploying your solution.  If these instructions are omitted or unclear, points may be deducted.
5. Submit artifacts by way of a pull request to the private repository. All artifacts must be committed **before 9:00 a.m. Pacific time on Thursday, January 18th, 2024**.
6. Attach an Apache License 2.0 to your pull request.

# Evaluation

| Criteria | Max Points |
| --- | --- |
| User Stories - Solution meets requirements | 15 |
| Architecture and technical design | 4 |
| Code quality & maintainability | 4 |
| Build & Deployment Instructions | 2 |
|                                   |            |
|                                   |            |
|                                   |            |
| Total | 25 |
