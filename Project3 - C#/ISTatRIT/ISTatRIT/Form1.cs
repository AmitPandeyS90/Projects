using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#region class
namespace ISTatRIT
{

    #region class ISTWindow
    public partial class ISTWindow : Form
    {

        #region constructor()
        public ISTWindow()
        {
            InitializeComponent();
            populate();
        }
        #endregion  

        #region populate()
        private void populate()
        {

            //colors used from material palette 
            Color color1 = System.Drawing.ColorTranslator.FromHtml("#4CAF50");
            Color color2 = System.Drawing.ColorTranslator.FromHtml("#FF5722");
            Color color3 = System.Drawing.ColorTranslator.FromHtml("#FF9800");
            Color color4 = System.Drawing.ColorTranslator.FromHtml("#448AFF");
            Color color5 = System.Drawing.ColorTranslator.FromHtml("#0097A7");
            Color color6 = System.Drawing.ColorTranslator.FromHtml("#FF5252");
            Color color7 = System.Drawing.ColorTranslator.FromHtml("#00796B");
            Color color8 = System.Drawing.ColorTranslator.FromHtml("#FFC107");
            Color color9 = System.Drawing.ColorTranslator.FromHtml("#7C4DFF");
            Color color10 = System.Drawing.ColorTranslator.FromHtml("#CDDC39");
            //array containing colors
            System.Drawing.Color[] colorS = { color1, color2, color3, color4, color5, color6, color7, color8, color9, color10 };
            Random rnd = new Random();


            // get About information-----------------------------------------------------
            string jsonAbout = getRestData("/about/");

            // need a way to get the JSON form into an About object
            About about = JToken.Parse(jsonAbout).ToObject<About>();

            // start displaying the About object information on the screen
            rtbAboutDescription.Text = about.description;
            rtbAuthor.Text = about.quoteAuthor;
            rtbTitle.Text = about.title;
            rtbEmployerComment.Text = about.quote;



            // get undergraduate information-----------------------------------------------------
            lblUhead.Text = "Our Undergraduate Degrees";
            String jsonDegrees = getRestData("/degrees/");
            Degrees degrees = JToken.Parse(jsonDegrees).ToObject<Degrees>();

            for (int i = 0; i < degrees.undergraduate.Count; i++)
            {
                Button btnCurrent;
                //String b  = "btn" + degrees.undergraduate[i].degreeName ;
                if (i == 2)
                {
                    btnCurrent = btnCIT;
                }
                else if (i == 1)
                {
                    btnCurrent = btnHCC;
                }
                else
                {
                    btnCurrent = btnWMC;
                }

                btnCurrent.Text = (degrees.undergraduate[i].title) + (Environment.NewLine) + (Environment.NewLine) + (degrees.undergraduate[i].description);
                int randomColorIndex = rnd.Next(0, 9);
                btnCurrent.BackColor = colorS[randomColorIndex];
            }

            // get graduate information-----------------------------------------------------

            lblGhead.Text = "Our Graduate Degrees";
            for (int i = 0; i < (degrees.graduate.Count); i++)
            {
                if (!(degrees.graduate[i].degreeName).Equals("graduate advanced certificates"))
                {
                    Button btnCurrent;
                    //String b  = "btn" + degrees.undergraduate[i].degreeName ;


                    if (i == 2)
                    {
                        btnCurrent = btnNSA;
                    }
                    else if (i == 1)
                    {
                        btnCurrent = btnHCI;
                    }
                    else
                    {
                        btnCurrent = btnIST;
                    }

                    btnCurrent.Text = (degrees.graduate[i].title) + (Environment.NewLine) + (Environment.NewLine) + (degrees.graduate[i].description);
                    int randomColorIndex = rnd.Next(0, 9);
                    btnCurrent.BackColor = colorS[randomColorIndex];
                }
            } //end for loop

            // get certificate information-----------------------------------------------------
            lblCertHead.Text = "Our Graduate Advanced Certificates";
            int xspace = 590;
            int yspace = 1;
            for (int i = 0; i < degrees.graduate[3].availableCertificates.Count; i++)
            {
                LinkLabel link1 = new LinkLabel();
                
                link1.Location = new Point(xspace, (yspace + 8) * 70);
                link1.Text = degrees.graduate[3].availableCertificates[i];
                link1.Size = new Size(700, 60);
                link1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
                link1.Font = new Font("Franklin Gothic Book", 12, FontStyle.Underline);
                yspace += 1;
                xspace = xspace -40;
                link1.LinkClicked += new LinkLabelLinkClickedEventHandler(LinkedLabelClicked);
                this.tabGraduate.Controls.Add(link1);

            }


            // get minor information-----------------------------------------------------

            //Reads data from api using RESTful
            String jsonMinors = getRestData("/minors/");
            Minors minors = JToken.Parse(jsonMinors).ToObject<Minors>();

            //Set the flowLayout Panel
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.FlowDirection = FlowDirection.LeftToRight;
            panel.WrapContents = true;
            panel.Size = new Size(1200, 600);
            panel.Location = new Point(100, 80);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Padding = new Padding(5);

            //Add flowlayout panel on the Tab
            tabMinors.Controls.Add(panel);
            var ctr = minors.UgMinors.Count;


            //Reads the data from the list and dynamically generate buttons
            for (int i = 0; i < ctr; i++)
            {
                Button b = new Button();
                b.Size = new Size(245, 251);
                int randomColorIndex = rnd.Next(0, 9);
                b.BackColor = colorS[randomColorIndex];
                b.Margin = new Padding(25);
                b.Name = minors.UgMinors[i].name;
                b.Text = minors.UgMinors[i].title;

                b.MouseClick += new MouseEventHandler(getDetails);

                //add button to the panel
                panel.Controls.Add(b);
            }

            // get Employment information-----------------------------------------------------
            //Reads data from api using RESTful
            String jsonEmployment = getRestData("/employment/");
            Employment empl = JToken.Parse(jsonEmployment).ToObject<Employment>();

            lblEmplTitle.Text = empl.introduction.title;

            lblEmplSubHead.Text = empl.introduction.content[0].title;
            rtbEmplDesc.Text = empl.introduction.content[0].description;

            lblCoopSubHead.Text = empl.introduction.content[1].title;
            rtbCoopDesc.Text = empl.introduction.content[1].description;

            FlowLayoutPanel fl = new FlowLayoutPanel();
            //fl.BorderStyle = BorderStyle.FixedSingle;
            fl.FlowDirection = FlowDirection.LeftToRight;
            fl.Location = new Point(83, 300);
            fl.WrapContents = true;
            fl.Size = new Size(1400, 200);
            fl.Padding = new Padding(5);

            tabEmployment.Controls.Add(fl);


            for (int i = 0; i < empl.degreeStatistics.statistics.Count; i++)
            {
                Label lbl = new Label();
                lbl.Size = new Size(300, 300);
                lbl.BorderStyle = BorderStyle.FixedSingle;
                lbl.BackColor = colorS[i];
                lbl.Margin = new Padding(20);
                lbl.Padding = new Padding(20);
                lbl.TextAlign = ContentAlignment.TopCenter;
                lbl.Text = empl.degreeStatistics.statistics[i].value + "\n\n" + empl.degreeStatistics.statistics[i].description;
                lbl.Margin = new Padding(10);
                //lbl.Paint = new Color(200, 150, 100);
                fl.Controls.Add(lbl);
            }

            //employer table
            Label lblEmplList = new Label();
            //lblEmplList.Size = new Size(300, 300);
            lblEmplList.Location = new Point(300, 510);
            lblEmplList.Text = empl.employers.title;


            //generate list of employers using read array method
            RichTextBox rtbEmpList = new RichTextBox();
            rtbEmpList.Location = new Point(252, 540);
            rtbEmpList.Size = new Size(400, 200);
            rtbEmpList.Text = readArray(empl.employers.employerNames);

            //add label and rich text box to the tab
            tabEmployment.Controls.Add(lblEmplList);
            tabEmployment.Controls.Add(rtbEmpList);


            //coop table
            Label lblCoopList = new Label();
            lblCoopList.Location = new Point(900, 510);
            //lblCoopList.Size = new Size(300, 300);
            lblCoopList.Text = empl.careers.title;


            //generate list of coop using read array method
            RichTextBox rtbCoopList = new RichTextBox();
            rtbCoopList.Location = new Point(697, 540);
            rtbCoopList.Size = new Size(400, 200);
            rtbCoopList.Text = readArray(empl.employers.employerNames);

            //add label and rich text box to the tab
            tabEmployment.Controls.Add(lblCoopList);
            tabEmployment.Controls.Add(rtbCoopList);

            // get Employer Grid Table-----------------------------------------------------

            Label tabTableHead = new Label();
            tabTableHead.Location = new Point(549, 26);
            tabTableHead.Size = new Size(300, 50);


            //create new dataview grid for employer table
            DataGridView dgvEmpl = new DataGridView();
            dgvEmpl.Size = new Size(1299, 509);
            dgvEmpl.Location = new Point(55, 84);
            dgvEmpl.Font = new Font("Trebuchet MS", 9, FontStyle.Regular);
            dgvEmpl.RowTemplate.Height = 25;

            //row header styling
            dgvEmpl.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#009688");
            dgvEmpl.EnableHeadersVisualStyles = false;
            dgvEmpl.ColumnHeadersHeight = 30;
            dgvEmpl.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEmpl.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEmpl.ColumnHeadersDefaultCellStyle.Font = new Font("Trebuchet MS", 12, FontStyle.Regular);



            //create new dataview grid for coop table
            DataGridView dgvCoop = new DataGridView();
            dgvCoop.Size = new Size(1299, 509);
            dgvCoop.Location = new Point(55, 84);
            dgvCoop.Font = new Font("Trebuchet MS", 9, FontStyle.Regular);

            //row header styling
            dgvCoop.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#AFB42B");
            dgvCoop.EnableHeadersVisualStyles = false;
            dgvCoop.ColumnHeadersHeight = 30;
            dgvCoop.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCoop.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCoop.ColumnHeadersDefaultCellStyle.Font = new Font("Trebuchet MS", 12, FontStyle.Regular);

            //heading for employer table
            tabTableHead.Text = empl.employmentTable.title;
            tabEmployerTable.Controls.Add(tabTableHead);

            //column head for employer table
            dgvEmpl.Columns.Add("newColumnName", "Employer");
            dgvEmpl.Columns.Add("newColumnName", "Degree");
            dgvEmpl.Columns.Add("newColumnName", "City");
            dgvEmpl.Columns.Add("newColumnName", "Title");
            dgvEmpl.Columns.Add("newColumnName", "Start Date");

            //count the columns in the gridview
            int columnCount = dgvEmpl.ColumnCount;

            //set the size of each column to the content size of displayed cells
            for (int i = 0; i < columnCount; i++)
            {

                dgvEmpl.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            //function to populate the Employer grid
            populateEmplGrid(empl.employmentTable.professionalEmploymentInformation, dgvEmpl);

            //add gridview to the tab
            tabEmployerTable.Controls.Add(dgvEmpl);


            tabTableHead.Text = empl.coopTable.title;
            tabCoop.Controls.Add(tabTableHead);

            //column head for coop table
            dgvCoop.Columns.Add("newColumnName", "Employer");
            dgvCoop.Columns.Add("newColumnName", "Degree");
            dgvCoop.Columns.Add("newColumnName", "City");
            dgvCoop.Columns.Add("newColumnName", "Term");

            columnCount = dgvCoop.ColumnCount;
            //set the size of each column to the content size of displayed cells
            for (int i = 0; i < columnCount; i++)
            {
                dgvCoop.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }



            //function to populate the coop grid
            populateCoopGrid(empl.coopTable.coopInformation, dgvCoop);

            //add gridview to the tab
            tabCoop.Controls.Add(dgvCoop);

            // get Map-----------------------------------------------------
            //Button link for map
            Button btnShowMap = new Button();
            btnShowMap.Size = new Size(367, 44);
            btnShowMap.Location = new Point(1092, 15);
            btnShowMap.Text = "Where our Students Work";

            //EventHandler for ButtonClick
            btnShowMap.MouseClick += new MouseEventHandler(createMap);


            tabTables.Controls.Add(btnShowMap);

            // Our People--------------------------------------------------------------

            //Reads data from api using RESTful
            String jsonPeople = getRestData("/people/");
            People facStaff = JToken.Parse(jsonPeople).ToObject<People>();

            Label lblFacStaffTitle = new Label();
            lblFacStaffTitle.Text = facStaff.title;
            lblFacStaffTitle.Size = new Size(200, 80);
            lblFacStaffTitle.Location = new Point(650, 15);
            lblFacStaffTitle.Font = new Font("Trebuchet MS", 20, FontStyle.Regular);
            lblFacStaffTitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
            lblFacStaffTitle.ForeColor = Color.White;
            lblFacStaffTitle.TextAlign = ContentAlignment.MiddleCenter;

            //add Title to the tab
            tabOurPeople.Controls.Add(lblFacStaffTitle);

            //groupbox for faculty
            GroupBox gbFaculty = new GroupBox();
            gbFaculty.Size = new Size(420, 740);
            gbFaculty.Location = new Point(0, 0);
            gbFaculty.Text = "Faculty";



            //create flowlayout for faculty
            FlowLayoutPanel flFac = new FlowLayoutPanel();
            flFac.FlowDirection = FlowDirection.LeftToRight;
            flFac.WrapContents = true;
            flFac.Size = new Size(390, 730);
            flFac.Location = new Point(5, 20);
            flFac.BorderStyle = BorderStyle.FixedSingle;
            flFac.AutoScroll = true;
            flFac.BackColor = System.Drawing.ColorTranslator.FromHtml("#009688");



            //create buttons for faculty
            foreach (Faculty fac in facStaff.faculty)
            {
                //create new button and style 
                Button btnFac = new Button();
                btnFac.Size = new Size(163, 38);
                btnFac.Margin = new Padding(10);
                //btnFac.BackColor = System.Drawing.ColorTranslator.FromHtml("#795548");
                btnFac.NotifyDefault(false);
                btnFac.ForeColor = Color.White;
                btnFac.Font = new Font("Trebuchet MS", 10, FontStyle.Regular);
                btnFac.BackgroundImage = ISTatRIT.Properties.Resources.buttonImage;
                btnFac.FlatAppearance.BorderSize = 0;
                btnFac.FlatStyle = FlatStyle.Popup;

                //add text to button and name it same as username
                btnFac.Text = fac.name;
                btnFac.Name = fac.username;

                //create a mouseClick event getFacultyStaff
                btnFac.MouseClick += new MouseEventHandler(getFacultyDetails);

                //place buttons in Flowlayout panel
                flFac.Controls.Add(btnFac);
            }

            //put flowlayoutpanel in groupbox
            gbFaculty.Controls.Add(flFac);

            //put items on the tab
            tabOurPeople.Controls.Add(gbFaculty);

            //groupbox for staff
            GroupBox gbStaff = new GroupBox();
            gbStaff.Size = new Size(400, 740);
            gbStaff.Location = new Point(1129, 0);
            gbStaff.Text = "Staff";

            //create flowlayout for Staff
            FlowLayoutPanel flStaff = new FlowLayoutPanel();
            flStaff.FlowDirection = FlowDirection.LeftToRight;
            flStaff.WrapContents = true;
            flStaff.Size = new Size(390, 730);
            flStaff.Location = new Point(5, 20);
            flStaff.BorderStyle = BorderStyle.FixedSingle;
            flStaff.AutoScroll = true;
            flStaff.BackColor = System.Drawing.ColorTranslator.FromHtml("#009688");

            //create buttons for staff
            foreach (Staff staff in facStaff.staff)
            {
                //create new button and style 
                Button btnStaff = new Button();
                btnStaff.Size = new Size(163, 38);
                btnStaff.Margin = new Padding(10);
                btnStaff.NotifyDefault(false);
                btnStaff.ForeColor = Color.White;
                btnStaff.Font = new Font("Trebuchet MS", 10, FontStyle.Regular);
                btnStaff.BackgroundImage = ISTatRIT.Properties.Resources.buttonImage;
                btnStaff.FlatAppearance.BorderSize = 0;
                btnStaff.FlatStyle = FlatStyle.Popup;

                //add text to button and name it same as username
                btnStaff.Text = staff.name;
                btnStaff.Name = staff.username;

                //create a mouseClick event getFacultyStaff
                btnStaff.MouseClick += new MouseEventHandler(getStaffDetails);

                //place buttons in Flowlayout panel
                flStaff.Controls.Add(btnStaff);
            }

            //put flowlayoutpanel in groupbox
            gbStaff.Controls.Add(flStaff);

            //put items on the tab
            tabOurPeople.Controls.Add(gbStaff);

            // ResearchBy InterestArea--------------------------------------------------------------
            //Reads data from api using RESTful
            String jsonResearch = getRestData("/research/");
            Research research = JToken.Parse(jsonResearch).ToObject<Research>();

            //create new heading label
            Label lblResearch = new Label();
            lblResearch.Text = "Research By Interest Area";
            //lblResearch.Size = new Size(350, 50);
            lblResearch.AutoSize = true;
            lblResearch.Location = new Point(650, 15);
            lblResearch.Font = new Font("Trebuchet MS", 20, FontStyle.Regular);
            lblResearch.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
            lblResearch.ForeColor = Color.White;
            lblResearch.TextAlign = ContentAlignment.MiddleCenter;


            //add Title to the tab
            tabByInterestArea.Controls.Add(lblResearch);

            //create flowlayout for InterestArea
            FlowLayoutPanel flnterestArea = new FlowLayoutPanel();
            flnterestArea.FlowDirection = FlowDirection.LeftToRight;
            flnterestArea.WrapContents = true;
            flnterestArea.Size = new Size(390, 710);
            flnterestArea.Location = new Point(5, 30);
            flnterestArea.BorderStyle = BorderStyle.FixedSingle;
            flnterestArea.AutoScroll = true;
            flnterestArea.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

            //Add flowlayoutpanel to tab
            tabByInterestArea.Controls.Add(flnterestArea);

            foreach (ByInterestArea anArea in research.byInterestArea)
            {
                //create new button and style 
                Button btnAnInterestArea = new Button();
                btnAnInterestArea.Size = new Size(115, 115);
                btnAnInterestArea.Margin = new Padding(15);
                //btnFac.BackColor = System.Drawing.ColorTranslator.FromHtml("#795548");
                btnAnInterestArea.NotifyDefault(false);
                btnAnInterestArea.ForeColor = Color.White;
                btnAnInterestArea.Font = new Font("Trebuchet MS", 10, FontStyle.Bold);
                btnAnInterestArea.BackgroundImage = ISTatRIT.Properties.Resources.roundButton;
                btnAnInterestArea.FlatAppearance.BorderSize = 0;
                btnAnInterestArea.FlatAppearance.BorderColor = Color.White;
                btnAnInterestArea.FlatStyle = FlatStyle.Flat;
                btnAnInterestArea.TextAlign = ContentAlignment.MiddleCenter;



                //Name the button and put text in it
                btnAnInterestArea.Text = anArea.areaName;
                btnAnInterestArea.Name = anArea.areaName;

                btnAnInterestArea.MouseClick += new MouseEventHandler(getResearchInterest);

                //add button to flowlayout panel
                flnterestArea.Controls.Add(btnAnInterestArea);
            }

            // research by faculty -----------------------------------------------------------
            Label lblFaculty = new Label();
            lblFaculty.Text = "Research By Faculty";
            //lblResearch.Size = new Size(350, 50);
            lblFaculty.AutoSize = true;
            lblFaculty.Location = new Point(650, 15);
            lblFaculty.Font = new Font("Trebuchet MS", 20, FontStyle.Regular);
            lblFaculty.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
            lblFaculty.ForeColor = Color.White;
            lblFaculty.TextAlign = ContentAlignment.MiddleCenter;


            
            //add Title to the tab
            tabByFaculty.Controls.Add(lblFaculty);

            FlowLayoutPanel flResearchByFaculty = new FlowLayoutPanel();
            flResearchByFaculty.FlowDirection = FlowDirection.LeftToRight;
            flResearchByFaculty.WrapContents = true;
            flResearchByFaculty.Size = new Size(390, 710);
            flResearchByFaculty.Location = new Point(5, 30);
            flResearchByFaculty.BorderStyle = BorderStyle.FixedSingle;
            flResearchByFaculty.AutoScroll = true;
            flResearchByFaculty.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

            //ListBox lbRBF = new ListBox();
            //lbRBF.Size = new Size(390, 700);
            //lbRBF.Location = new Point(10, 35);
            //lbRBF.Font = new Font("Trebuchet MS", 12, FontStyle.Regular);

            //add listbox to flowlayout panel
            flResearchByFaculty.Controls.Add(lbRBF);

            //display faculty names on the listbox
            foreach (ByFaculty aFaculty in research.byFaculty)
            {
                lbRBF.Items.Add(aFaculty.facultyName);
            }

            //add flowlayout panel to split container
            tabByFaculty.Controls.Add(flResearchByFaculty);

            // resources -----------------------------------------------------------


            //Reads data from api using RESTful
            String jsonResources = getRestData("/resources/");
            Resources resource = JToken.Parse(jsonResources).ToObject<Resources>();

            // StudyAbroad -----------------------------------------------------------
            lblResourceTitle.Text = "";
            lblResourceDesc.Text = "";

            //set the title and description for study abroad
            lblResourceTitle.Text = resource.studyAbroad.title;
            lblResourceDesc.Text = resource.studyAbroad.description;

            foreach (Place aPlace in resource.studyAbroad.places)
            {
                //create label for title
                Label lblPlaceTitle = new Label();
                lblPlaceTitle.Text = aPlace.nameOfPlace;
                //lblPlaceTitle.Margin = new Padding(50);
                lblPlaceTitle.Size = new Size(300, 100);
                lblPlaceTitle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
                lblPlaceTitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
                lblPlaceTitle.ForeColor = Color.White;
                lblPlaceTitle.TextAlign = ContentAlignment.MiddleCenter;

                //create label for description
                Label lblPlaceDesc = new Label();
                lblPlaceDesc.Text = aPlace.description;
                lblPlaceDesc.Size = new Size(280, 300);
                lblPlaceDesc.Location = new Point(10, 110);
                lblPlaceDesc.Margin = new Padding(20);
                lblPlaceDesc.Font = new Font("Trebuchet MS", 11, FontStyle.Bold);
                lblPlaceDesc.ForeColor = Color.White;
                lblPlaceDesc.TextAlign = ContentAlignment.TopLeft;

                Panel panelAplace = new Panel();
                panelAplace.Size = new Size(300, 400);
                panelAplace.Margin = new Padding(50);
                panelAplace.BorderStyle = BorderStyle.FixedSingle;
                panelAplace.BackColor = System.Drawing.ColorTranslator.FromHtml("#795548");


                //put title label and description label on the Panel
                panelAplace.Controls.Add(lblPlaceTitle);
                panelAplace.Controls.Add(lblPlaceDesc);


                //put panel on the flowlayout
                flStudyAbroad.AutoScroll = true;
                flStudyAbroad.WrapContents = false;
                flStudyAbroad.Controls.Add(panelAplace);
            }

            // StudentServices -----------------------------------------------------------


            //Professional Advisors       
            //create label for title
            Label lblPATitle = new Label();
            lblPATitle.Text = resource.studentServices.professonalAdvisors.title;
            //lblPlaceTitle.Margin = new Padding(50);
            lblPATitle.Size = new Size(300, 100);
            lblPATitle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            lblPATitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
            lblPATitle.ForeColor = Color.White;
            lblPATitle.TextAlign = ContentAlignment.MiddleCenter;

            //create a string of name dept and email of professional advisors
            string strAnAdvisor = null;
            foreach (AdvisorInformation anAdvisor in resource.studentServices.professonalAdvisors.advisorInformation)
            {
                strAnAdvisor += anAdvisor.name + "\n" + anAdvisor.department + "\n" + anAdvisor.email + "\n\n";
            }

            //create label for description
            Label lblPADesc = new Label();
            lblPADesc.Text = strAnAdvisor;
            lblPADesc.Size = new Size(280, 600);
            lblPADesc.Location = new Point(10, 110);
            lblPADesc.Margin = new Padding(20);
            lblPADesc.Font = new Font("Trebuchet MS", 11, FontStyle.Bold);
            lblPADesc.ForeColor = Color.White;
            lblPADesc.TextAlign = ContentAlignment.TopLeft;

            Panel panelPA = new Panel();
            panelPA.Size = new Size(300, 400);
            panelPA.Margin = new Padding(50);
            panelPA.BorderStyle = BorderStyle.FixedSingle;
            panelPA.BackColor = System.Drawing.ColorTranslator.FromHtml("#795548");
            panelPA.AutoScroll = true;

            //put title label and description label on the Panel
            panelPA.Controls.Add(lblPATitle);
            panelPA.Controls.Add(lblPADesc);


            //put panel on the flowlayout
            flStudentServices.AutoScroll = true;
            flStudentServices.WrapContents = false;
            flStudentServices.Controls.Add(panelPA);

            //Faculty Advisors
            //create label for title
            Label lblFATitle = new Label();
            lblFATitle.Text = resource.studentServices.facultyAdvisors.title;
            //lblPlaceTitle.Margin = new Padding(50);
            lblFATitle.Size = new Size(300, 100);
            lblFATitle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            lblFATitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
            lblFATitle.ForeColor = Color.White;
            lblFATitle.TextAlign = ContentAlignment.MiddleCenter;

            //create label for description
            Label lblFADesc = new Label();
            lblFADesc.Text = resource.studentServices.facultyAdvisors.description;
            lblFADesc.Size = new Size(280, 600);
            lblFADesc.Location = new Point(10, 110);
            lblFADesc.Margin = new Padding(20);
            lblFADesc.Font = new Font("Trebuchet MS", 11, FontStyle.Bold);
            lblFADesc.ForeColor = Color.White;
            lblFADesc.TextAlign = ContentAlignment.TopLeft;

            Panel panelFA = new Panel();
            panelFA.Size = new Size(300, 400);
            panelFA.Margin = new Padding(50);
            panelFA.BorderStyle = BorderStyle.FixedSingle;
            panelFA.BackColor = System.Drawing.ColorTranslator.FromHtml("#795548");
            panelFA.AutoScroll = true;

            //put title label and description label on the Panel
            panelFA.Controls.Add(lblFATitle);
            panelFA.Controls.Add(lblFADesc);


            //put panel on the flowlayout
            //flStudentServices.AutoScroll = true;
            //flStudentServices.WrapContents = false;
            flStudentServices.Controls.Add(panelFA);

            //ISTMinor Advising
            //create label for title
            Label lblMATitle = new Label();
            lblMATitle.Text = resource.studentServices.istMinorAdvising.title;
            //lblPlaceTitle.Margin = new Padding(50);
            lblMATitle.Size = new Size(300, 100);
            lblMATitle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            lblMATitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
            lblMATitle.ForeColor = Color.White;
            lblMATitle.TextAlign = ContentAlignment.MiddleCenter;

            String strMinorAdvisor = null;
            foreach (MinorAdvisorInformation aMinorAdvisor in resource.studentServices.istMinorAdvising.minorAdvisorInformation)
            {
                strMinorAdvisor += aMinorAdvisor.advisor + "\n" + aMinorAdvisor.title + "\n" + aMinorAdvisor.email + "\n\n";
            }

            //create label for description
            Label lblMADesc = new Label();
            lblMADesc.Text = strMinorAdvisor;
            lblMADesc.Size = new Size(280, 600);
            lblMADesc.Location = new Point(10, 110);
            lblMADesc.Margin = new Padding(20);
            lblMADesc.Font = new Font("Trebuchet MS", 11, FontStyle.Bold);
            lblMADesc.ForeColor = Color.White;
            lblMADesc.TextAlign = ContentAlignment.TopLeft;

            Panel panelMA = new Panel();
            panelMA.Size = new Size(300, 400);
            panelMA.Margin = new Padding(50);
            panelMA.BorderStyle = BorderStyle.FixedSingle;
            panelMA.BackColor = System.Drawing.ColorTranslator.FromHtml("#795548");
            panelMA.AutoScroll = true;

            //put title label and description label on the Panel
            panelMA.Controls.Add(lblMATitle);
            panelMA.Controls.Add(lblMADesc);


            //put panel on the flowlayout
            //flStudentServices.AutoScroll = true;
            //flStudentServices.WrapContents = false;
            flStudentServices.Controls.Add(panelMA);


            //Tutor and Lab information
            //create label for title
            Label lblTutorLabTitle = new Label();
            lblTutorLabTitle.Text = resource.tutorsAndLabInformation.title;
            //lblPlaceTitle.Margin = new Padding(50);
            lblTutorLabTitle.Size = new Size(300, 100);
            lblTutorLabTitle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            lblTutorLabTitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
            lblTutorLabTitle.ForeColor = Color.White;
            lblTutorLabTitle.TextAlign = ContentAlignment.MiddleCenter;

            //create label for description
            Label lblTutorLabDesc = new Label();
            lblTutorLabDesc.Text = resource.tutorsAndLabInformation.description;
            lblTutorLabDesc.Size = new Size(280, 600);
            lblTutorLabDesc.Location = new Point(10, 110);
            lblTutorLabDesc.Margin = new Padding(20);
            lblTutorLabDesc.Font = new Font("Trebuchet MS", 11, FontStyle.Bold);
            lblTutorLabDesc.ForeColor = Color.White;
            lblTutorLabDesc.TextAlign = ContentAlignment.TopLeft;


            Panel panelTutorLab = new Panel();
            panelTutorLab.Size = new Size(300, 400);
            panelTutorLab.Margin = new Padding(50);
            panelTutorLab.BorderStyle = BorderStyle.FixedSingle;
            panelTutorLab.BackColor = System.Drawing.ColorTranslator.FromHtml("#795548");
            panelTutorLab.AutoScroll = true;

            //put title label and description label on the Panel
            panelTutorLab.Controls.Add(lblTutorLabTitle);
            panelTutorLab.Controls.Add(lblTutorLabDesc);


            //put panel on the flowlayout
            //flStudentServices.AutoScroll = true;
            //flStudentServices.WrapContents = false;
            flTutorLab.Controls.Add(panelTutorLab);


            //Student Ambassador           
            foreach (SubSectionContent aContent in resource.studentAmbassadors.subSectionContent)
            {
                //create label for title
                Label lblContentTitle = new Label();
                lblContentTitle.Text = aContent.title;
                //lblPlaceTitle.Margin = new Padding(50);
                lblContentTitle.Size = new Size(300, 100);
                lblContentTitle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
                lblContentTitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
                lblContentTitle.ForeColor = Color.White;
                lblContentTitle.TextAlign = ContentAlignment.MiddleCenter;

                //create label for description
                Label lblContentDesc = new Label();
                lblContentDesc.Text = aContent.description;
                lblContentDesc.Size = new Size(280, 600);
                lblContentDesc.Location = new Point(10, 110);
                lblContentDesc.Margin = new Padding(20);
                lblContentDesc.Font = new Font("Trebuchet MS", 11, FontStyle.Bold);
                lblContentDesc.ForeColor = Color.White;
                lblContentDesc.TextAlign = ContentAlignment.TopLeft;

                Panel panelAcontent = new Panel();
                panelAcontent.Size = new Size(300, 400);
                panelAcontent.Margin = new Padding(50);
                panelAcontent.BorderStyle = BorderStyle.FixedSingle;
                panelAcontent.BackColor = System.Drawing.ColorTranslator.FromHtml("#795548");
                panelAcontent.AutoScroll = true;
                //panelAcontent.AutoScrollMinSize = AutoScrollMinSize;

                //put title label and description label on the Panel
                panelAcontent.Controls.Add(lblContentTitle);
                panelAcontent.Controls.Add(lblContentDesc);


                //put panel on the flowlayout
                flSA.AutoScroll = true;
                flSA.WrapContents = false;
                flSA.Controls.Add(panelAcontent);
            }

            //Forms
            //Graduate FOrms
            //create label for title
            Label lblFormTitle = new Label();
            lblFormTitle.Text = "Links for Graduate Forms";
            //lblPlaceTitle.Margin = new Padding(50);
            lblFormTitle.Size = new Size(300, 100);
            lblFormTitle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            lblFormTitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.TextAlign = ContentAlignment.MiddleCenter;

            Panel panelGLinks = new Panel();
            panelGLinks.Size = new Size(300, 800);
            panelGLinks.Margin = new Padding(50);
            panelGLinks.BorderStyle = BorderStyle.FixedSingle;
            panelGLinks.BackColor = System.Drawing.ColorTranslator.FromHtml("#795548");
            panelGLinks.AutoScroll = true;

            //panelAcontent.AutoScrollMinSize = AutoScrollMinSize;

            int n = 1;
            foreach (GraduateForm aGForm in resource.forms.graduateForms)
            {
                //create Linklabel for form links
                LinkLabel lnkAGForm = new LinkLabel();
                lnkAGForm.Text = aGForm.formName;
                lnkAGForm.LinkClicked += new LinkLabelLinkClickedEventHandler(gLinkClicked);
                lnkAGForm.Font = new Font("Franklin Gothic Book", 11, FontStyle.Underline);
                lnkAGForm.LinkColor = Color.White;
                lnkAGForm.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;


                lnkAGForm.Height = 10;
                lnkAGForm.Width = 50;
                lnkAGForm.AutoSize = true;
                lnkAGForm.Location = new Point(20, (n + 1) * 50);

                //put title label and description label on the Panel
                panelGLinks.Controls.Add(lblFormTitle);
                panelGLinks.Controls.Add(lnkAGForm);


                //put panel on the flowlayout
                flForms.AutoScroll = true;
                //flForms.WrapContents = false;
                flForms.Controls.Add(panelGLinks);

                //increment the y location
                n += 1;
            }

            //Undergraduate FOrms
            //create label for title
            Label lblUFormTitle = new Label();
            lblUFormTitle.Text = "Links for Undergraduate Forms";
            //lblPlaceTitle.Margin = new Padding(50);
            lblUFormTitle.Size = new Size(300, 100);
            lblUFormTitle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            lblUFormTitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
            lblUFormTitle.ForeColor = Color.White;
            lblUFormTitle.TextAlign = ContentAlignment.MiddleCenter;

            Panel panelULinks = new Panel();
            panelULinks.Size = new Size(300, 800);
            panelULinks.Margin = new Padding(50);
            panelULinks.BorderStyle = BorderStyle.FixedSingle;
            panelULinks.BackColor = System.Drawing.ColorTranslator.FromHtml("#795548");
            panelULinks.AutoScroll = true;

            //panelAcontent.AutoScrollMinSize = AutoScrollMinSize;

            int k = 1;
            foreach (UndergraduateForm aUForm in resource.forms.undergraduateForms)
            {
                //create Linklabel for form links
                LinkLabel lnkAUForm = new LinkLabel();
                lnkAUForm.Text = aUForm.formName;
                lnkAUForm.LinkClicked += new LinkLabelLinkClickedEventHandler(gLinkClicked);
                lnkAUForm.Font = new Font("Franklin Gothic Book", 11, FontStyle.Underline);
                lnkAUForm.LinkColor = Color.White;
                lnkAUForm.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
                lnkAUForm.Height = 10;
                lnkAUForm.Width = 50;
                lnkAUForm.AutoSize = true;
                lnkAUForm.Location = new Point(20, (k + 1) * 50);

                //put title label and description label on the Panel
                panelULinks.Controls.Add(lblUFormTitle);
                panelULinks.Controls.Add(lnkAUForm);


                //put panel on the flowlayout
                flForms.AutoScroll = true;
                //flForms.WrapContents = false;
                flForms.Controls.Add(panelULinks);

                //increment the y location
                k += 1;
            }

            //Coop Enrollment
            foreach (EnrollmentInformationContent aEIContent in resource.coopEnrollment.enrollmentInformationContent)
            {
                //create label for title
                Label lblCoopEnrolTitle = new Label();
                lblCoopEnrolTitle.Text = aEIContent.title;
                lblCoopEnrolTitle.Padding = new Padding(20);
                lblCoopEnrolTitle.Size = new Size(320, 100);

                lblCoopEnrolTitle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
                lblCoopEnrolTitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF9800");
                lblCoopEnrolTitle.ForeColor = Color.White;
                lblCoopEnrolTitle.TextAlign = ContentAlignment.MiddleCenter;
                //lblCoopEnrolTitle.Margin = new Padding(20);

                //create label for description
                Label lblCoopEnrolDesc = new Label();
                lblCoopEnrolDesc.Text = aEIContent.description;
                lblCoopEnrolDesc.Size = new Size(300, 600);
                lblCoopEnrolDesc.Location = new Point(10, 110);
                lblCoopEnrolDesc.Padding = new Padding(30);
                lblCoopEnrolDesc.Font = new Font("Trebuchet MS", 11, FontStyle.Bold);
                lblCoopEnrolDesc.ForeColor = Color.White;
                lblCoopEnrolDesc.TextAlign = ContentAlignment.TopLeft;

                Panel panelCoopEnrol = new Panel();
                panelCoopEnrol.Size = new Size(300, 400);
                panelCoopEnrol.Margin = new Padding(50);
                panelCoopEnrol.BorderStyle = BorderStyle.FixedSingle;
                panelCoopEnrol.BackColor = System.Drawing.ColorTranslator.FromHtml("#795548");
                panelCoopEnrol.AutoScroll = true;
                //panelAcontent.AutoScrollMinSize = AutoScrollMinSize;

                //put title label and description label on the Panel
                panelCoopEnrol.Controls.Add(lblCoopEnrolTitle);
                panelCoopEnrol.Controls.Add(lblCoopEnrolDesc);


                //put panel on the flowlayout
                flCoopEnrollment.AutoScroll = true;
                flCoopEnrollment.WrapContents = false;
                flCoopEnrollment.Controls.Add(panelCoopEnrol);
            }

            //News
            //YearNews 
            String jsonNews = getRestData("/news/");
            News news = JToken.Parse(jsonNews).ToObject<News>();

            //find the number of news-year item
            int newsCountYear = news.year.Count;

            //set the columns to 2
            tlNewsYear.ColumnCount = (2);

            //find how many rows are needed
            if ((newsCountYear % 2) == 0)
            {
                tlNewsYear.RowCount = (newsCountYear / 2);
            }
            else
            {
                tlNewsYear.RowCount = (int)Math.Round((double)(newsCountYear / 2), MidpointRounding.AwayFromZero);
            }

            //read the news from api and store in each cell of tableLayout
            foreach (Year thisYear in news.year)
            {

                RichTextBox rtbNewsYearItem = new RichTextBox();
                rtbNewsYearItem.Text = (thisYear.date + "\n" + thisYear.title + "\n\n" + thisYear.description);
                rtbNewsYearItem.Font = new Font("Sans Serif", 12, FontStyle.Regular);
                //add flowlayout to the tablelayout
                tlNewsYear.Controls.Add(rtbNewsYearItem);
                rtbNewsYearItem.Dock = DockStyle.Fill;

                tlNewsYear.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            }

            //Quarter news
            //find the number of news-quarter item
            int newsCountQuarter = news.quarter.Count;

            //set the columns to 2
            tlNewsQuarter.ColumnCount = (2);

            //find how many rows are needed
            if ((newsCountQuarter % 2) == 0)
            {
                tlNewsQuarter.RowCount = (newsCountQuarter / 2);
            }
            else if ((newsCountQuarter % 2) != 0)
            {
                tlNewsQuarter.RowCount = (int)Math.Round((double)(newsCountQuarter / 2), MidpointRounding.AwayFromZero);
            }

            //read the news from api and store in each cell of tableLayout
            foreach (Quarter thisQuarter in news.quarter)
            {

                RichTextBox rtbNewsQuarterItem = new RichTextBox();
                rtbNewsQuarterItem.Text = (thisQuarter.date + "\n" + thisQuarter.title + "\n\n" + thisQuarter.description);
                rtbNewsQuarterItem.Font = new Font("Sans Serif", 12, FontStyle.Regular);

                //add flowlayout to the tablelayout
                tlNewsQuarter.Controls.Add(rtbNewsQuarterItem);
                rtbNewsQuarterItem.Dock = DockStyle.Fill;

                //divide rows in equal size
                tlNewsQuarter.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            }

            //Older news
            RichTextBox rtbNewsOlderItem = new RichTextBox();
            tlOlderNews.Controls.Add(rtbNewsOlderItem);
            rtbNewsOlderItem.Dock = DockStyle.Fill;

            string oldItems = null;
            foreach (Older anOlderItem in news.older)
            {
                oldItems += anOlderItem.date + "\n" + anOlderItem.title + "\n\n" + anOlderItem.description + "\n\n";
            }

            rtbNewsOlderItem.Text = oldItems;
            rtbNewsOlderItem.Font = new Font("Sans Serif", 12, FontStyle.Regular);

            //Footer

            String jsonFooter = getRestData("/footer/");
            Footer footer = JToken.Parse(jsonFooter).ToObject<Footer>();

            //Panel panelQuickLinks = new Panel();
            //panelQuickLinks.Location = new Point(1527, 71);
            //panelQuickLinks.Size = new Size(300,700);
            //panelQuickLinks.Font = new Font("Times New Roman",12,FontStyle.Regular);
            int q = 1;


            foreach (QuickLink linkItems in footer.quickLinks)
            {
                LinkLabel aLink = new LinkLabel();
                aLink.Text = linkItems.title;
                //aLink.Location = new Point(10,((q+2)*10));
                //aLink.Size = new Size(100,100);
                aLink.LinkClicked += new LinkLabelLinkClickedEventHandler(footerQuickLinks);
                panelQuickLinks.Controls.Add(aLink);
                q += 1;
                aLink.Font = new Font("Times New Roman", 14, FontStyle.Italic);
                aLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;


                aLink.Height = 30;
                aLink.Width = 400;
                aLink.AutoSize = true;
                aLink.Location = new Point(10, ((q + 2) * 50));

                Console.WriteLine(linkItems.title);
            }

            //social
            //panelSocial.BackColor = lblUFormTitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#607D8B");
            panelSocial.ForeColor = lblUFormTitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#faebd7");

            //title
            Label lblSocialTitle = new Label();
            lblSocialTitle.Location = new Point();
            lblSocialTitle.Size = new Size(2000, 20);
            lblSocialTitle.TextAlign = ContentAlignment.TopCenter;
            lblSocialTitle.Font = new Font("Courier", 12, FontStyle.Bold);
            //lblSocialTitle.BorderStyle = BorderStyle.FixedSingle;
            lblSocialTitle.Text = footer.social.title;

            panelSocial.Controls.Add(lblSocialTitle);


            //tweet header
            Label lblSocialTweetHeader = new Label();
            lblSocialTweetHeader.Location = new Point(5, 50);
            lblSocialTweetHeader.Size = new Size(2000, 20);
            lblSocialTweetHeader.Text = "Tweet of the day";
            lblSocialTweetHeader.TextAlign = ContentAlignment.TopCenter;

            panelSocial.Controls.Add(lblSocialTweetHeader);

            //tweet content
            Label lblSocialTweet = new Label();
            lblSocialTweet.Location = new Point(5, 70);
            lblSocialTweet.Size = new Size(2000, 20);
            lblSocialTweet.TextAlign = ContentAlignment.TopCenter;
            lblSocialTweet.Font = new Font("Courier", 10, FontStyle.Regular);
            //lblSocialTweet.BorderStyle = BorderStyle.FixedSingle;
            lblSocialTweet.Text = footer.social.tweet;

            panelSocial.Controls.Add(lblSocialTweet);

            //tweetby
            Label lblSocialTweetBy = new Label();
            lblSocialTweetBy.Location = new Point(1150, 90);
            lblSocialTweetBy.Size = new Size(2000, 20);
            lblSocialTweetBy.Font = new Font("Courier", 9, FontStyle.Italic);
            //lblSocialTweetBy.BorderStyle = BorderStyle.FixedSingle;

            lblSocialTweetBy.Text = "Tweet By: " + footer.social.by;

            panelSocial.Controls.Add(lblSocialTweetBy);

            //twitter
            LinkLabel lLinkSocialTwitter = new LinkLabel();
            lLinkSocialTwitter.Location = new Point(5, 110);
            lLinkSocialTwitter.Size = new Size(2000, 20);
            lLinkSocialTwitter.Font = new Font("Courier", 11, FontStyle.Italic);
            lLinkSocialTwitter.LinkClicked += new LinkLabelLinkClickedEventHandler(getTwitter);
            lLinkSocialTwitter.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            lLinkSocialTwitter.Text = "Twitter";
            lLinkSocialTwitter.TextAlign = ContentAlignment.MiddleCenter;
            //lLinkSocialTwitter.BorderStyle = BorderStyle.FixedSingle;

            panelSocial.Controls.Add(lLinkSocialTwitter);

            //facebook
            LinkLabel lLinkSocialFacebook = new LinkLabel();
            lLinkSocialFacebook.Location = new Point(5, 130);
            lLinkSocialFacebook.Size = new Size(2000, 20);
            lLinkSocialFacebook.Font = new Font("Courier", 11, FontStyle.Italic);
            lLinkSocialFacebook.LinkClicked += new LinkLabelLinkClickedEventHandler(getTwitter);
            lLinkSocialFacebook.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            lLinkSocialFacebook.Text = "Facebook";
            lLinkSocialFacebook.TextAlign = ContentAlignment.MiddleCenter;
            //lLinkSocialFacebook.BorderStyle = BorderStyle.FixedSingle;

            panelSocial.Controls.Add(lLinkSocialFacebook);

            //News
            //facebook
            LinkLabel lLinkSocialNews = new LinkLabel();
            lLinkSocialNews.Location = new Point(5, 160);
            lLinkSocialNews.Size = new Size(2000, 20);
            lLinkSocialNews.Font = new Font("Courier", 11, FontStyle.Italic);
            lLinkSocialNews.LinkClicked += new LinkLabelLinkClickedEventHandler(getTwitter);
            lLinkSocialNews.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            lLinkSocialNews.Text = "News";
            lLinkSocialNews.TextAlign = ContentAlignment.MiddleCenter;
            //lLinkSocialFacebook.BorderStyle = BorderStyle.FixedSingle;

            panelSocial.Controls.Add(lLinkSocialFacebook);

            //Copyright
            LinkLabel lLinkSocialCopyright = new LinkLabel();
            lLinkSocialCopyright.Location = new Point(5, 170);
            lLinkSocialCopyright.Size = new Size(200, 20);
            lLinkSocialCopyright.Font = new Font("Courier", 9, FontStyle.Italic);
            lLinkSocialCopyright.LinkClicked += new LinkLabelLinkClickedEventHandler(getTwitter);
            lLinkSocialCopyright.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            lLinkSocialCopyright.Text = footer.copyright.title;
            lLinkSocialCopyright.TextAlign = ContentAlignment.MiddleCenter;
            lLinkSocialCopyright.BorderStyle = BorderStyle.FixedSingle;
            lLinkSocialCopyright.BackColor = Color.DarkOrange;

            panelSocial.Controls.Add(lLinkSocialCopyright);

            //RIT
            Label lLinkSocialRIT = new Label();
            lLinkSocialRIT.Location = new Point(200, 170);
            lLinkSocialRIT.Size = new Size(300, 20);
            lLinkSocialRIT.Font = new Font("Courier", 9, FontStyle.Italic);
            //lLinkSocialRIT.LinkClicked += new LinkLabelLinkClickedEventHandler(getTwitter);
            lLinkSocialRIT.Text = "Rochester Institute of Technology";
            lLinkSocialRIT.TextAlign = ContentAlignment.MiddleCenter;
            lLinkSocialRIT.BorderStyle = BorderStyle.FixedSingle;
            lLinkSocialRIT.BackColor = Color.DarkOrange;

            panelSocial.Controls.Add(lLinkSocialRIT);

            //All RIghts Reserved
            Label lLinkSocialARR = new Label();
            lLinkSocialARR.Location = new Point(500, 170);
            lLinkSocialARR.Size = new Size(300, 20);
            lLinkSocialARR.Font = new Font("Courier", 9, FontStyle.Italic);
            //lLinkSocialRIT.LinkClicked += new LinkLabelLinkClickedEventHandler(getTwitter);
            lLinkSocialARR.Text = "All Rights Reserved";
            lLinkSocialARR.TextAlign = ContentAlignment.MiddleCenter;
            lLinkSocialARR.BorderStyle = BorderStyle.FixedSingle;
            lLinkSocialARR.BackColor = Color.DarkOrange;

            panelSocial.Controls.Add(lLinkSocialARR);

            //Copyright Infringement
            LinkLabel lLinkSocialCI = new LinkLabel();
            lLinkSocialCI.Location = new Point(800, 170);
            lLinkSocialCI.Size = new Size(300, 20);
            lLinkSocialCI.Font = new Font("Courier", 9, FontStyle.Italic);
            lLinkSocialCI.LinkClicked += new LinkLabelLinkClickedEventHandler(getTwitter);
            lLinkSocialCI.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            lLinkSocialCI.Text = "Copyright Infringement";
            lLinkSocialCI.TextAlign = ContentAlignment.MiddleCenter;
            lLinkSocialCI.BorderStyle = BorderStyle.FixedSingle;
            lLinkSocialCI.BackColor = Color.DarkOrange;

            panelSocial.Controls.Add(lLinkSocialCI);

            //Privacy Statement
            LinkLabel lLinkSocialPS = new LinkLabel();
            lLinkSocialPS.Location = new Point(1100, 170);
            lLinkSocialPS.Size = new Size(300, 20);
            lLinkSocialPS.Font = new Font("Courier", 9, FontStyle.Italic);
            lLinkSocialPS.LinkClicked += new LinkLabelLinkClickedEventHandler(getTwitter);
            lLinkSocialPS.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            lLinkSocialPS.Text = "Privacy Statement";
            lLinkSocialPS.TextAlign = ContentAlignment.MiddleCenter;
            lLinkSocialPS.BorderStyle = BorderStyle.FixedSingle;
            lLinkSocialPS.BackColor = Color.DarkOrange;

            panelSocial.Controls.Add(lLinkSocialPS);

            //Disclaimer
            LinkLabel lLinkSocialD = new LinkLabel();
            lLinkSocialD.Location = new Point(1400, 170);
            lLinkSocialD.Size = new Size(300, 20);
            lLinkSocialD.Font = new Font("Courier", 9, FontStyle.Italic);
            lLinkSocialD.LinkClicked += new LinkLabelLinkClickedEventHandler(getTwitter);
            lLinkSocialD.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            lLinkSocialD.Text = "Disclaimer";
            lLinkSocialD.TextAlign = ContentAlignment.MiddleCenter;
            lLinkSocialD.BorderStyle = BorderStyle.FixedSingle;
            //lLinkSocialND.Controls.Add(lLinkSocialD);
            lLinkSocialD.BackColor = Color.DarkOrange;

            panelSocial.Controls.Add(lLinkSocialD);

            //NonDiscrimination
            LinkLabel lLinkSocialND = new LinkLabel();
            lLinkSocialND.Location = new Point(1700, 170);
            lLinkSocialND.Size = new Size(280, 20);
            lLinkSocialND.Font = new Font("Courier", 9, FontStyle.Italic);
            lLinkSocialND.LinkClicked += new LinkLabelLinkClickedEventHandler(getTwitter);
            lLinkSocialND.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            lLinkSocialND.Text = "Non-Discrimination";
            lLinkSocialND.TextAlign = ContentAlignment.MiddleCenter;
            lLinkSocialND.BorderStyle = BorderStyle.FixedSingle;
            //lLinkSocialND.Controls.Add(lLinkSocialD);
            lLinkSocialND.BackColor = Color.DarkOrange;

            panelSocial.Controls.Add(lLinkSocialND);


            //contact Us
            wbContactUs.Navigate("http://ist.rit.edu/api/contactForm/");
            wbContactUs.ScriptErrorsSuppressed = true;


        } //end populate function
        #endregion

        #region get RIT footer Links
        private void getTwitter(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String jsonFooter = getRestData("/footer/");
            Footer footer = JToken.Parse(jsonFooter).ToObject<Footer>();

            string linkSocial = ((LinkLabel)sender).Text;

            Console.WriteLine(linkSocial);

            if (linkSocial.Equals("Twitter"))
            {

                System.Diagnostics.Process.Start(footer.social.twitter);
            }
            else if (linkSocial.Equals("Facebook"))
            {
                System.Diagnostics.Process.Start(footer.social.facebook);
            }
            else if (linkSocial.Equals("News"))
            {
                System.Diagnostics.Process.Start(footer.news);
            }
            else if (linkSocial.Equals("Copyright Infringement"))
            {
                System.Diagnostics.Process.Start("http://www.rit.edu/copyright.html");
            }
            else if (linkSocial.Equals("Rochester Institute of Technology"))
            {
                System.Diagnostics.Process.Start("http://www.rit.edu/");
            }
            else if (linkSocial.Equals("Privacy Statement"))
            {
                System.Diagnostics.Process.Start("http://www.rit.edu/privacystatement.html");
            }
            else if (linkSocial.Equals("Disclaimer"))
            {
                System.Diagnostics.Process.Start("http://www.rit.edu/disclaimer.html");
            }
            else if (linkSocial.Equals("Non-Discrimination"))
            {
                System.Diagnostics.Process.Start("http://www.rit.edu/nondiscrimination.html");
            }

        }

        #endregion

        #region footer links
        private void footerQuickLinks(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String jsonFooter = getRestData("/footer/");
            Footer footer = JToken.Parse(jsonFooter).ToObject<Footer>();

            string text = ((LinkLabel)sender).Text;

            foreach (QuickLink linkItems in footer.quickLinks)
            {
                if (linkItems.title.Equals(text))
                {
                    System.Diagnostics.Process.Start(linkItems.href);
                }
            }

        }
        #endregion


        #region getRESTful()
        private string getRestData(string url)
        {
            string baseUri = "http://ist.rit.edu/api";

            // connect to the API
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUri + url);
            try
            {
                WebResponse response = request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException we)
            {
                // Something goes wrong, get the error response, then do something with it
                WebResponse err = we.Response;
                using (Stream responseStream = err.GetResponseStream())
                {
                    StreamReader r = new StreamReader(responseStream, Encoding.UTF8);
                    string errorText = r.ReadToEnd();
                    // display or log error
                    Console.WriteLine(errorText);
                }
                throw;
            }
        } //end getRESTful method
        #endregion

        #region get Minor Details
        private void getDetails(object sender, MouseEventArgs e)
        {
            //Reads data from api using RESTful
            String jsonMinors = getRestData("/minors/");
            Minors minors = JToken.Parse(jsonMinors).ToObject<Minors>();

            Button b = (Button)sender;
            String btnName = b.Name;

            //loop to parse through all minors
            for (int i = 0; i < minors.UgMinors.Count; i++)
            {
                //match the button clicked with minor in json. If found display details in messagebox
                if (minors.UgMinors[i].name == btnName)
                {
                    int count_of_minors = minors.UgMinors[i].courses.Count;
                    String title = minors.UgMinors[i].title;
                    String course = null;

                    //parse through the courses
                    for (int j = 0; j < count_of_minors; j++)
                    {
                        course += minors.UgMinors[i].courses[j] + "\n";
                    }
                    MessageBox.Show(minors.UgMinors[i].description + "\n" + "\n" + "Courses- \n" + course, title);
                }
            }
        } //end of getDetails method
        #endregion

        #region get Undergraduate Concentration
        /*
         * function that looks for the degree and pulls out concentration
         */
        private void getConcentration(object sender, MouseEventArgs e)
        {
            String ConcList = null;
            Button b = (Button)sender;
            String btnName = b.Name;

            //convert button name to a string and to a lowercase
            btnName = (btnName.Substring(3)).ToLower();

            String jsonDegrees = getRestData("/degrees/");
            Degrees degrees = JToken.Parse(jsonDegrees).ToObject<Degrees>();

            for (int i = 0; i < degrees.undergraduate.Count; i++)
            {
                if (degrees.undergraduate[i].degreeName.Equals(btnName))
                {
                    ConcList = degrees.undergraduate[i].title + "\n\n";
                    foreach (String conc in degrees.undergraduate[i].concentrations)
                    {
                        ConcList = ConcList + ("\u2022") + (" ") + conc + "\n";
                    }

                    MessageBox.Show(ConcList, "Concentrations", MessageBoxButtons.OK);
                }
            }

            for (int i = 0; i < (degrees.graduate.Count); i++)
            {
                //MessageBox.Show(btnName);
                if (degrees.graduate[i].degreeName.Equals(btnName))
                {
                    ConcList = degrees.graduate[i].title + "\n\n";
                    foreach (String conc in degrees.graduate[i].concentrations)
                    {
                        ConcList = ConcList + ("\u2022") + (" ") + conc + "\n";
                    }
                    MessageBox.Show(ConcList, "Concentrations", MessageBoxButtons.OK);
                }
            }

        }//end of getConcentration
        #endregion

        #region Graduate certificates
        private void LinkedLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String jsonDegrees = getRestData("/degrees/");
            Degrees degrees = JToken.Parse(jsonDegrees).ToObject<Degrees>();

            string text = ((LinkLabel)sender).Text;
            if (text.Equals("Web Development Advanced certificate"))
            {
                System.Diagnostics.Process.Start("http://www.rit.edu/programs/web-development-adv-cert");
            }
            else
            {
                System.Diagnostics.Process.Start("http://www.rit.edu/programs/networking-planning-and-design-adv-cert");
            }
        }
        #endregion

        #region Empl and Coop table methods
        private void populateCoopGrid(List<CoopInformation> coopinfo, DataGridView dgvCoop)
        {
            for (var i = 0; i < coopinfo.Count; i++)
            {
                dgvCoop.Rows.Add();
                dgvCoop.Rows[i].Cells[0].Value =
                    coopinfo[i].employer;
                dgvCoop.Rows[i].Cells[1].Value =
                    coopinfo[i].degree;
                dgvCoop.Rows[i].Cells[2].Value =
                    coopinfo[i].city;
                dgvCoop.Rows[i].Cells[3].Value =
                    coopinfo[i].term;

            }
            //row styling

        }

        private void populateEmplGrid(List<ProfessionalEmploymentInformation> emplInfo, DataGridView dgvEmpl)
        {
            for (var i = 0; i < emplInfo.Count; i++)
            {
                dgvEmpl.Rows.Add();
                dgvEmpl.Rows[i].Cells[0].Value =
                    emplInfo[i].employer;
                dgvEmpl.Rows[i].Cells[1].Value =
                    emplInfo[i].degree;
                dgvEmpl.Rows[i].Cells[2].Value =
                    emplInfo[i].city;
                dgvEmpl.Rows[i].Cells[3].Value =
                    emplInfo[i].title;

            }


        }
        #endregion

        #region read employer and coop array
        private String readArray(List<string> list)
        {
            String str = null;
            foreach (String listItem in list)
            {
                str += (Environment.NewLine) + "\u2022" + listItem;
            }

            return str;
        }
        #endregion

        #region create Map on a new form
        private void createMap(object sender, MouseEventArgs e)
        {

            //create new form for map
            Form mapForm = new Form();
            mapForm.Size = new Size(1200, 700);
            mapForm.Location = new Point(150, 150);

            //create new browser
            WebBrowser mapBrowser = new WebBrowser();
            //mapBrowser.Size = new Size(700, 600);
            mapBrowser.Navigate("http://ist.rit.edu/api/map");
            mapBrowser.Dock = DockStyle.Fill;

            //add webbrowser to the form
            mapForm.Controls.Add(mapBrowser);

            //show the form
            mapForm.Show();
        }
        #endregion

        #region Faculty details
        private void getFacultyDetails(object sender, MouseEventArgs e)
        {
            String jsonPeople = getRestData("/people/");
            People facStaff = JToken.Parse(jsonPeople).ToObject<People>();

            Button btnwhoAmI = (Button)sender;

            //List<People> searchThisList = facStaff.faculty;

            foreach (Faculty fac in facStaff.faculty)
            {


                if (fac.username == btnwhoAmI.Name)
                {
                    //PictureBox pbxPeoplePic = new PictureBox();
                    //pbxPeoplePic.Location = new Point(670,30);
                    //pbxPeoplePic.Size = new Size(236,236);

                    //pbxPeoplePic.BackgroundImageLayout = ImageLayout.Stretch;
                    //pbxPeoplePic.BorderStyle = BorderStyle.Fixed3D;

                    pbxPeoplePic.Image = null;
                    pbxPeoplePic.InitialImage = null;
                    pbxPeoplePic.ImageLocation = null;

                    pbxPeoplePic.ImageLocation = fac.imagePath;
                    tabOurPeople.Controls.Add(pbxPeoplePic);

                    lblFacStaffName.Text = fac.name;
                    lblFacStaffTitle.Text = fac.title;
                    lblInterestArea.Text = fac.interestArea.ToUpper();
                    lblOffice.Text = fac.office;
                    lblPhone.Text = fac.phone;
                    lblEmail.Text = fac.email;
                    lblWebsite.Text = fac.website;
                    lblTwitter.Text = fac.twitter;
                    lblFacebook.Text = fac.facebook;

                    break;
                }

            }

        }
        #endregion

        #region get staff details
        private void getStaffDetails(object sender, MouseEventArgs e)
        {
            String jsonPeople = getRestData("/people/");
            People facStaff = JToken.Parse(jsonPeople).ToObject<People>();

            Button btnwhoAmI = (Button)sender;

            //List<People> searchThisList = facStaff.faculty;

            foreach (Staff staf in facStaff.staff)
            {
                if (staf.username == btnwhoAmI.Name)
                {
                    //PictureBox pbxPeoplePic = new PictureBox();
                    //pbxPeoplePic.Location = new Point(670,30);
                    //pbxPeoplePic.Size = new Size(236,236);

                    //pbxPeoplePic.BackgroundImageLayout = ImageLayout.Stretch;
                    //pbxPeoplePic.BorderStyle = BorderStyle.Fixed3D;

                    pbxPeoplePic.Image = null;
                    pbxPeoplePic.InitialImage = null;
                    pbxPeoplePic.ImageLocation = null;

                    pbxPeoplePic.ImageLocation = staf.imagePath;
                    tabOurPeople.Controls.Add(pbxPeoplePic);

                    lblFacStaffName.Text = staf.name;
                    lblFacStaffTitle.Text = staf.title;
                    lblInterestArea.Text = staf.interestArea.ToUpper();
                    lblOffice.Text = staf.office;
                    lblPhone.Text = staf.phone;
                    lblEmail.Text = staf.email;
                    lblWebsite.Text = staf.website;
                    lblTwitter.Text = staf.twitter;
                    lblFacebook.Text = staf.facebook;

                    break;
                }

            }
        }
        #endregion

        #region Research Interest
        private void getResearchInterest(object sender, MouseEventArgs e)
        {
            //read api
            String jsonResearch = getRestData("/research/");
            Research research = JToken.Parse(jsonResearch).ToObject<Research>();

            //find which button is clicked
            Button whichButtonClicked = (Button)sender;


            //create richtextbox to show citation
            //RichTextBox rtbCitation = new RichTextBox();
            //rtbCitation.Size = new Size(1071, 670);
            //rtbCitation.Location = new Point(423, 70);
            rtbCitation.BackColor = System.Drawing.ColorTranslator.FromHtml("#8BC34A");
            rtbCitation.ForeColor = Color.White;
            rtbCitation.Font = new Font("Trebuchet MS", 12, FontStyle.Regular);
            String citationText = "";

            //clear the contents of rich textbox
            rtbCitation.SelectAll();
            rtbCitation.Text = "";


            foreach (ByInterestArea anInterestArea in research.byInterestArea)
            {
                if (whichButtonClicked.Name == (anInterestArea.areaName))
                {
                    foreach (String aCitation in anInterestArea.citations)
                    {
                        citationText += aCitation + "\n";
                    }
                    rtbCitation.SelectAll();
                    rtbCitation.SelectionIndent = 15;
                    rtbCitation.SelectionRightIndent = 15;
                    rtbCitation.SelectionLength = 0;
                    rtbCitation.Text = citationText;

                }
            }

            tabByInterestArea.Controls.Add(rtbCitation);
        }
        #endregion

        #region drag and drop
        private void lbRBF_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (this.lbRBF.SelectedItem != null)
            {
                //pick up from flowlayout
                DoDragDrop(this.lbRBF.SelectedItem, DragDropEffects.Copy);
            }
        }

        private void lblResearchDetails_DragEnter_1(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Text"))
            {
                e.Effect = DragDropEffects.Copy;
                lblResearchDetails.BackColor = Color.LightBlue;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lblResearchDetails_DragDrop(object sender, DragEventArgs e)
        {
            String jsonResearch = getRestData("/research/");
            Research research = JToken.Parse(jsonResearch).ToObject<Research>();

            lblResearchDetails.Text = "";

            string findMe = (string)e.Data.GetData("Text");

            foreach (ByFaculty aFaculty in research.byFaculty)
            {
                if (aFaculty.facultyName.Equals(findMe))
                {
                    foreach (string aCita in aFaculty.citations)
                    {
                        lblResearchDetails.Text += aCita + "\n";
                    }

                }
            }
        }


        #endregion

        #region Resource sub tab functions
        private void tabResourceDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            String jsonResources = getRestData("/resources/");
            Resources resource = JToken.Parse(jsonResources).ToObject<Resources>();

            if (tabResourceDetail.SelectedTab == tabResourceDetail.TabPages["tabTutorLab"])
            {
                //reset
                lblResourceTitle.Text = "";
                lblResourceDesc.Text = "";
                linkLabelTL.Visible = true;

                lblResourceTitle.Text = resource.tutorsAndLabInformation.title;
                tlResource.Controls.Remove(appFormLink);
                tlResource.Controls.Add(linkLabelTL, 1, 0);

            }
            else if (tabResourceDetail.SelectedTab == tabResourceDetail.TabPages["tabStudentServices"])
            {
                //reset
                lblResourceTitle.Text = "";
                lblResourceDesc.Text = "";
                linkLabelTL.Visible = false;
                appFormLink.Visible = false;
                //set the title and description for Student Services
                lblResourceTitle.Text = resource.studentServices.academicAdvisors.title;
                lblResourceDesc.Text = resource.studentServices.academicAdvisors.description;
            }
            else if (tabResourceDetail.SelectedTab == tabResourceDetail.TabPages["tabStudyAbroad"])
            {
                lblResourceTitle.Text = "";
                lblResourceDesc.Text = "";
                linkLabelTL.Visible = false;
                appFormLink.Visible = false;
                //set the title and description for study abroad
                lblResourceTitle.Text = resource.studyAbroad.title;
                lblResourceDesc.Text = resource.studyAbroad.description;
            }

            else if (tabResourceDetail.SelectedTab == tabResourceDetail.TabPages["tabStudentAm"])
            {
                lblResourceTitle.Text = "";
                lblResourceDesc.Text = "";

                linkLabelTL.Visible = false;
                appFormLink.Visible = true;

                lblResourceTitle.Text = resource.studentAmbassadors.title;
                lblResourceDesc.Text = resource.studentAmbassadors.note;
                tlResource.Controls.Remove(linkLabelTL);
                tlResource.Controls.Add(appFormLink, 1, 0);
                appFormLink.LinkClicked += new LinkLabelLinkClickedEventHandler(OpenAppForm);

            }

            else if (tabResourceDetail.SelectedTab == tabResourceDetail.TabPages["tabForms"])
            {
                lblResourceTitle.Text = "";
                lblResourceDesc.Text = "";
                linkLabelTL.Visible = false;
                appFormLink.Visible = false;
                //set the title and description for study abroad
                lblResourceTitle.Text = "Forms";

            }
            else if (tabResourceDetail.SelectedTab == tabResourceDetail.TabPages["tabCoopEnrol"])
            {
                lblResourceTitle.Text = "";
                lblResourceDesc.Text = "";
                linkLabelTL.Visible = false;
                appFormLink.Visible = false;

                lblResourceTitle.Text = resource.coopEnrollment.title;
            }

        }

        private void OpenAppForm(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String jsonResources = getRestData("/resources/");
            Resources resource = JToken.Parse(jsonResources).ToObject<Resources>();
            System.Diagnostics.Process.Start(resource.studentAmbassadors.applicationFormLink);
        }
        #endregion

        #region Tutor and Lab Link
        private void linkLabelTL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.istlabs.rit.edu");
        }
        #endregion

        #region graduate forms link click event handler
        private void gLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string text = ((LinkLabel)sender).Text;
            //MessageBox.Show(text);

            String jsonResources = getRestData("/resources/");
            Resources resource = JToken.Parse(jsonResources).ToObject<Resources>();

            foreach (GraduateForm aGForm in resource.forms.graduateForms)
            {
                if (aGForm.formName.Equals(text))
                {
                    //Do we need to make an asset folder with all pdfs
                    //System.Diagnostics.Process.Start(aGForm.href);
                    break;
                }
            }


        }
        #endregion

    }//end class
    #endregion
}//end namespace
#endregion