var dataObj;	//Object to load JSON file
var imgEle;		//Element that holds the image


/*
 * function to display form
 */
function displayForm(fBody){

	var nForm = document.createElement('form');
	nForm.setAttribute("id", "nameForm");
	nForm.setAttribute("action", "javascript:genPage(document.body);");
	nForm.setAttribute("onsubmit", "return validateForm();");

	var fLabel = document.createElement("label");
	fLabel.appendChild(document.createTextNode("Name"));

	var ftext = document.createElement("input");
	ftext.setAttribute("type", "text");
	ftext.setAttribute("id", "fname");

	var fInput = document.createElement("input");
	fInput.setAttribute("type", "submit");
	fInput.setAttribute("value", "submit");

	nForm.appendChild(fLabel);
	nForm.appendChild(ftext);
	nForm.appendChild(fInput);

	fBody.appendChild(nForm);
}

function validateForm() {
		var valid = true;
				var fn = document.getElementById("fname").value
		 		if (fn == "") {
							alert('Please fill in your name!');
							return (!valid);
						}
				else {
					localStorage.setItem("name", fn);
					// var t = "Welcome" + fn + "Please answer the following:"
					// t = document.createTextNode(t);
					// document.getElementById("nameForm").appendChild(t);
					return valid;

		}
} //end of validate function


function pageLanding() {
	debugger;
	var mybody = document.getElementsByTagName('body')[0];

	if (localStorage.getItem("name")) {
		genPage(mybody);
	}
	else {
		displayForm(mybody);
	}

}
/*
 *  funtion that generates the page
 */
function genPage(mybody){

	//Object that will hold the JSON
	dataObj = new Object();

	//declare API to gather data from JSON
	var xmlhttp = new XMLHttpRequest();

	xmlhttp.onreadystatechange = function() {

		//checks if data from JSON file is loaded successfully
		if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {

			dataObj = JSON.parse(xmlhttp.responseText);

		}
	};

	//set to false for code to stop execution until JSON file is loaded. Error with true.
	xmlhttp.open("GET", "https://people.rit.edu/~ap6932/ISTE340/Project1/js/data.json", false);
	xmlhttp.send();

	//clears the localStorage
	//localStorage.clear();



	//header tag to hold heading
	var myheader = document.createElement('header');

	//creates H1 heading and the text
	var myh1 = document.createElement('h1');
 var x = ("Welcome " + window.localStorage.getItem("name") + ", Enjoy the world food!");
	myh1.appendChild( document.createTextNode( x ));

	//H1 tag added to header tag
	myheader.appendChild(myh1);

	mybody.appendChild(myheader);

	//creates form element
	var myForm = document.createElement('form');
	myForm.setAttribute('id', 'mainForm');
	myForm.setAttribute('onsubmit', "submitForm();return false;" );
	myForm.setAttribute('onreset', "resetHandler()" );


	//div for submit and reset button
	var submissionDiv = document.createElement('div');
	submissionDiv.setAttribute('id', 'submitDiv');

	//submit button
	var mySubmit = document.createElement('input');
	mySubmit.setAttribute('type', 'submit');
	mySubmit.setAttribute('value', 'Submit');

	//reset button
	var myReset = document.createElement('input');
	myReset.setAttribute('type', 'reset');
	myReset.setAttribute('value', 'Reset');

	submissionDiv.appendChild(mySubmit);
	submissionDiv.appendChild(myReset);

	//shows final choices
	var choicesDiv = document.createElement('div');
	choicesDiv.setAttribute('id', 'chDiv');
	var myH3 = document.createElement('h3');

	choicesDiv.appendChild(myH3);

	//div for showing images
	var imgDiv = document.createElement('div');
	imgDiv.setAttribute('id', 'imageChoice');

	imgEle = document.createElement('img');

	imgDiv.appendChild(imgEle);

	var roosImg = document.createElement("img");
	roosImg.setAttribute("src", "img/frame_0_delay-s.gif");
	roosImg.setAttribute("id", "rooster");
	roosImg.setAttribute("alt", "rooster");
	roosImg.setAttribute("style", "position: absolute; left:5px; bottom:100px;")
	roosImg.setAttribute("onclick", "moveit()");


	//getElementById('mainForm') returning null for some unknown reason, using alternate method
	myForm.appendChild(submissionDiv);

	mybody.appendChild(myForm);
	mybody.appendChild(choicesDiv);
	mybody.appendChild(imgDiv);
	mybody.appendChild(roosImg);



	//Calls displayNextQuestion method with argument that fetches the first key from the JSON Object (init);
	displayNextQuestion(Object.keys(dataObj)[0]);


}

function moveit(){
	var r = document.getElementById("rooster");
	if ((parseInt(r.style.left)) <= 1000 ) {


		r.style.left = parseInt(r.style.left) + 1 + 'px';
		setTimeout(function(){moveit();}, 30);

	}
	else {

		r.style.left = "2px";
	}

}

/*
 *  function reads the user selection and map it to the next question in JSON object, displays next question
 *  and calls "CreateOptions" function to display dropdown choices.
 */

function displayNextQuestion(choice) {


		//create select tag for droop down list
		var SEle = document.createElement('Select');
		SEle.setAttribute( 'id', choice );

		//Set onChange event on the Select that captures the choice
		SEle.setAttribute('onchange', 'getChoice(this)');

		//array pulled from JSON Object for the choice made in earlier question
		var arr = dataObj[choice];

		//traverse through the array, create H2 element for question and option element for choices
		for (var i=0; i < arr.length; i++){

			var myform;
			if (i == 0){

				imgEle.setAttribute('src', arr[i]);
			}
			//If it's a question
			else if (i==1) {

				myform = document.getElementById('mainForm');

				//Each question is in its own div
				myQdiv = document.createElement('div');

				//create h2 element for question
				var hEle = document.createElement("h2");
				var text = document.createTextNode(arr[i]);


				hEle.appendChild(text);
				myQdiv.appendChild(hEle);

			}

			// for choices
			else
			{
				createOptions(SEle, arr[i]);
			}

		} //end for loop

		myQdiv.appendChild(SEle);

		//div containing the question is inserted before the last div. Last div contains Submit and Reset button.
		myform.insertBefore(myQdiv, myform.lastChild);

} //end of displayNextQuestion()


/*
 * function is invoked whenever a choice on select statement is made/changed.
 * Parameter 'selectObj' is the question user picked a choice on.
 */
function getChoice(selectObj){


	//checks if the option selected was for last question.
	//If selected for previous question, rest of the questions are removed from the page
	while (selectObj != ((selectObj.parentNode.parentNode).lastChild).previousSibling.lastChild) {

	 		selectObj.parentNode.parentNode.removeChild(selectObj.parentNode.parentNode.lastChild.previousSibling);

	 }

	 //check to make sure item selected from drop down is not '-Select-'
	if (selectObj != null){

		//Extract the option picked from the drop down
		var choice = (selectObj[selectObj.selectedIndex].value);


		//If the key defined is a question
		if (dataObj[choice].length > 1){

			//EXTRA POINTS CANDIDATE- Using ternary operator to find if localStorage has a value, if yes get the value, concatenate and then set; else just set it
			((localStorage.getItem('choice'))? (localStorage.setItem('choice', localStorage.getItem('choice') + " " + choice)): (localStorage.setItem('choice', choice)));

			//Call method to display next question; pass the option chosen as argument
			displayNextQuestion(choice);
		}

		//if the key defined is just an image
		else {

			//sets the image for the chosen dish
			localStorage.setItem('choice', localStorage.getItem('choice') + " " + choice);

			//sets the end of the line images
			imgEle.setAttribute('src', (dataObj[choice])[0]);
		}



	} //end if
} //end of function getChoice

/*
 *	function adds options to the drop down
*/
function createOptions(SElement, option) {

	//creates dropdown choices
	var opt = document.createElement("option");
	opt.value = option;
	opt.text = option;

	//somehow this doesn't work??
	//opt.setAttribute('value', option);
	//opt.setAttribute('text', option);

	//adds the option
	SElement.appendChild(opt);

} //end of createOptions function

/*
 * Function invoked on submit
 * Reads from localStorage and displays on h3 tag
*/
function submitForm() {

	document.getElementsByTagName('h3')[0].appendChild(document.createTextNode("You ordered a delicious " + localStorage.getItem('choice') + "\r\n"));
}

/*
 * EXTRA POINTS CANDIDATE
 * function invoked on reset
 * Except for first question deletes the rest
 * clears localStorage
 * sets the image to intial image
 * counts the childnodes of h3 tag (final choices made) and removes them in the for loop
 */

function resetHandler() {

	//deletes all the questions until there is one left on the page
	while (document.getElementsByTagName('select').length != 1) {
		document.getElementById('mainForm').removeChild(document.getElementsByTagName('select')[document.getElementsByTagName('select').length - 1].parentNode.parentNode.lastChild.previousSibling);
	}

	//clears the localStorage
	localStorage.removeItem("choice");

	//sets the image to the first array in the Json onject
	imgEle.setAttribute('src', (dataObj[Object.keys(dataObj)[0]])[0]);

	var count = document.getElementsByTagName('h3')[0].childNodes.length;

	//Reset the textbox
	for(i = 0; i < count; i++) {

		document.getElementsByTagName('h3')[0].removeChild(document.getElementsByTagName('h3')[0].firstChild);
	}


}
