# CognitiveServices.Explorer
This project is mean to be used as a show case what Cognitive Services can do, built in client-side Blazor.

You can check out [live demo](https://jernejk.github.io/CognitiveServices.Explorer/)

**NOTE:** Currently not working on iOS. (https://github.com/mono/mono/issues/16986)
**NOTE 2:** PWA doesn't have an update logic and might require wiping data for jernejk.github.io website in your browser.

## Supported features

### Face API

#### Feature implemented

* Store base URL and subscription key to local storage
* Explore groups, identities and faces
* Show cURL requests
* Train group
* Check if group has been trained
* Detect
  * No facial features
  * From web cam
* Identify from detected faces
* Add/update/delete groups
* Add/update/delete people

#### Features planned in the coming weeks

* Add faces to person
  * From web cam
  * From URL
  * From file upload
* Update face (user data only)
* Detect faces
  * Emotions, gender, age, etc.
  * ~~From web cam~~
  * From file upload
  * From URL
* Find similar faces
* Verify face

### Other Cognitive Services in planning (coming months)

* Text Analyzer
* Translation
* Language Understanding
* Search
* Anomaly Detector 
* Computer Vision
* Ink Recognizer
* Form Recognizer
* Spell Checker
