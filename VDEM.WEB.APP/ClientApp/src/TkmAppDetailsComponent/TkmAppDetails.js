import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import * as vdemConst from "../VdemConstants";
import logo1 from "../Assests/img/Toyota-Logo.png";
import { Link } from "react-router-dom";

class TkmAppDetails extends Component {
  constructor(props) {
    super(props);
    this.state = {
      appDetails: [],
      TableData: [],
      systemCategory: [],
      localsystems: [],
      regionalsystems: [],
      globalsystems: [],
      engineeringsystems: [],
      systemsForTableData: [],
      purpose: "",
      AppNameHeader: "",
      ProdUrl: "",
      TestUrl: "",
      Message: "",
      LocalCount: "",
      GlobalCount: "",
      RegionalCount: "",
      EngineeringCount: "",
      TotalCount: "",
    };
  }

  componentDidMount() {
    this.refreshlist();
    var main_view_height = window.innerHeight - 88;
    document.getElementById("rowid").style.height =
      main_view_height - 210 + "px";
  }

  refreshlist() {
    var myHeaders = new Headers();
    var Token = sessionStorage.getItem("Token");
    myHeaders.append("Authorization", "Bearer " + Token);

    var requestOptions = {
      method: "GET",
      headers: myHeaders,
      redirect: "follow",
    };

    fetch(
      vdemConst.WEB_API_URL + "/TkmAppDetails/GetAppDetails",
      requestOptions
    )
      .then((response) => response.json())
      .then(
        (data) => {
          if (data.returnValue < 0) {
            return;
          }
          this.setState(
            {
              appDetails: data.appDetails,
              systemCategory: data.systemCategory,
              localsystems: data.localsystems,
              regionalsystems: data.regionalsystems,
              globalsystems: data.globalsystems,
              engineeringsystems: data.engineeringsystems,
            },
            () => {
              this.modifyData();
            }
          );
        },
        (error) => {}
      );
  }

  modifyData = () => {
    var appdetails = this.state.appDetails;
    var TableData = [];
    let lenArray = [];

    var LocalData = appdetails.filter((x) => x.APPLIATION_CATEGORY === "Local");
    var GlobalData = appdetails.filter(
      (x) => x.APPLIATION_CATEGORY === "Global"
    );
    var RegionalData = appdetails.filter(
      (x) => x.APPLIATION_CATEGORY === "Regional"
    );
    var EngineeringData = appdetails.filter(
      (x) => x.APPLIATION_CATEGORY === "Engineering system"
    );

    var LocalCount = LocalData.length;
    var GlobalCount = GlobalData.length;
    var RegionalCount = RegionalData.length;
    var EngineeringCount = EngineeringData.length;
    var TotalCount =
      LocalData.length + GlobalCount + RegionalCount + EngineeringCount;

    lenArray.push(LocalData.length);
    lenArray.push(GlobalData.length);
    lenArray.push(RegionalData.length);
    lenArray.push(EngineeringData.length);

    let Maximunlength = Math.max.apply(null, lenArray);

    if (LocalData.length === Maximunlength) {
      LocalData.forEach((element) => {
        var data = { local: element.APPLICATION_NAME };
        TableData.push(data);
      });

      TableData.forEach((ele, index) => {
        ele.regional = RegionalData[index].APPLICATION_NAME;
        ele.global = GlobalData[index].APPLICATION_NAME;
        ele.engineering = EngineeringData[index].APPLICATION_NAME;
      });
    } else if (GlobalData.length === Maximunlength) {
      GlobalData.forEach((element) => {
        var data = { global: element.APPLICATION_NAME };
        TableData.push(data);
      });

      TableData.forEach((ele, index) => {
        ele.regional = RegionalData[index].APPLICATION_NAME;
        ele.local = LocalData[index].APPLICATION_NAME;
        ele.engineering = EngineeringData[index].APPLICATION_NAME;
      });
    } else if (RegionalData.length === Maximunlength) {
      RegionalData.forEach((element) => {
        var data = { regional: element.APPLICATION_NAME };
        TableData.push(data);
      });

      TableData.forEach((ele, index) => {
        ele.local = LocalData[index].APPLICATION_NAME;
        ele.global = GlobalData[index].APPLICATION_NAME;
        ele.engineering = EngineeringData[index].APPLICATION_NAME;
      });
    } else if (EngineeringData.length === Maximunlength) {
      EngineeringData.forEach((element) => {
        var data = { engineering: element.APPLICATION_NAME };
        TableData.push(data);
      });

      TableData.forEach((ele, index) => {
        ele.regional =
          RegionalData[index] === undefined
            ? ""
            : RegionalData[index].APPLICATION_NAME;
        ele.global =
          GlobalData[index] === undefined
            ? ""
            : GlobalData[index].APPLICATION_NAME;
        ele.local =
          LocalData[index] === undefined
            ? ""
            : LocalData[index].APPLICATION_NAME;
      });
    }

    console.log(TableData);

    this.setState({
      TableData,
      LocalCount,
      GlobalCount,
      RegionalCount,
      EngineeringCount,
      TotalCount,
    });
  };

  onClickApp = (val) => {
    debugger;
    if (val !== "") {
      var appDetails = this.state.appDetails;
      var link = appDetails.find((x) => x.APPLICATION_NAME === val);
      var Purpsoe = link.PURPOSE;
      var URL = link.PRODUCTION_URL;
      var Message = "This is desktop Application";
      if (link.APPLICATION_TYPE === "WINDOWS") {
        document.getElementById("websiteBtn").style.display = "none";
        document.getElementById("message").style.display = "block";
      }
      if (link.APPLICATION_TYPE === "WEB") {
        document.getElementById("websiteBtn").style.display = "block";
        document.getElementById("message").style.display = "none";
      }
      this.setState({ Purpsoe, URL, Message }, () => {
        window.$("#ModalMessage").modal("show");
      });
    }
  };

  onClickGotoWebsite = () => {
    window.open(this.state.URL, "_blank");
    window.$("#ModalMessage").modal("hide");
  };

  onClickSystemCategory = (val, index) => {
    var systemsForTableData = [];
    var systemCategory = this.state.systemCategory;

    var purpose = "";
    var AppNameHeader = "";
    document.getElementById("systemHeader").style.display = "block";
    document.getElementById("table1").style.display = "none";
    document.getElementById("table2").style.display = "none";

    systemCategory.forEach((ele, ind) => {
      var AdvanceOk = document.getElementById("categoryBtn" + ind);
      AdvanceOk.classList = "";
      if (ind === index) {
        AdvanceOk.classList.add("btn", "btn-primary", "w-100", "mt-2");
      } else {
        AdvanceOk.classList.add("btn", "btn-grey", "w-100", "mt-2");
      }
    });

    var systemsForTableData = this.state.systemsForTableData;
    systemsForTableData.forEach((ele, ind) => {
      var AdvanceOk = document.getElementById("SystemBtn" + ind);
      AdvanceOk.classList.add("btn", "btn-grey", "w-100", "mt-2");
    });

    if (val === "Local") {
      systemsForTableData = this.state.localsystems;
    }
    if (val === "Global") {
      systemsForTableData = this.state.globalsystems;
    }
    if (val === "Regional") {
      systemsForTableData = this.state.regionalsystems;
    }
    if (val === "Engineering system") {
      systemsForTableData = this.state.engineeringsystems;
    }
    console.log(systemsForTableData);
    this.setState({ systemsForTableData, purpose, AppNameHeader });
  };

  onClickSystem = (val, index) => {
    var appDetails = this.state.appDetails;
    document.getElementById("table1").style.display = "";
    document.getElementById("table2").style.display = "";

    var systemsForTableData = this.state.systemsForTableData;
    systemsForTableData.forEach((ele, ind) => {
      var AdvanceOk = document.getElementById("SystemBtn" + ind);
      AdvanceOk.classList = "";
      if (ind === index) {
        AdvanceOk.classList.add("btn", "btn-primary", "w-100", "mt-2");
      } else {
        AdvanceOk.classList.add("btn", "btn-grey", "w-100", "mt-2");
      }
    });

    var link = appDetails.find((x) => x.APPLICATION_NAME === val);
    var purpose = link.PURPOSE;
    var ProdUrl = link.PRODUCTION_URL;
    var TestUrl = link.TEST_URL;
    var PrimaryMail = link.PRIMARY_SUPPORT_EMAIL;
    var PrimaryPhone = link.PRIMARY_SUPPORT_PHONE;
    var PrimayTeams = link.PRIMARY_SUPPORT_TEAMS;
    var SecondaryMail = link.SECONDARY_SUPPORT_EMAIL;
    var SecondaryPhone = link.SECONDARY_SUPPORT_PHONE;
    var SecondaryTeams = link.SECONDARY_SUPPORT_TEAMS;
    var AppNameHeader = val;

    this.setState({
      purpose,
      AppNameHeader,
      ProdUrl,
      TestUrl,
      PrimaryMail,
      PrimaryPhone,
      PrimayTeams,
      SecondaryMail,
      SecondaryPhone,
      SecondaryTeams,
    });
  };

  render() {
    const {
      TableData,
      systemCategory,
      localsystems,
      systemsForTableData,
      purpose,
      AppNameHeader,
      ProdUrl,
      TestUrl,
      LocalCount,
      GlobalCount,
      RegionalCount,
      EngineeringCount,
      TotalCount,
      PrimaryMail,
      PrimaryPhone,
      PrimayTeams,
      SecondaryMail,
      SecondaryPhone,
      SecondaryTeams,
    } = this.state;
    return (
      <div>
        <div
          className="row w-100"
          style={{
            position: "fixed",
            borderBlock: "1px solid black",
          }}
        >
          <div className="col-lg-6">
            <h4 className="mt-2">Manufacturing Systems</h4>
          </div>
          <div className="col-lg-4"></div>
          <div className="col-lg-2">
            <img
              src={logo1}
              alt={"logo1"}
              width="170"
              className="img-fluid"
              style={{ marginTop: "8px", marginLeft: "30px" }}
            />
          </div>
        </div>
        <div className="row">
          <div className="col-lg-2 mt-5"></div>
          <div className="col-lg-2 mt-5">
            <h4 style={{ textAlign: "right", marginTop: "18%" }}>
              Apps Count:
            </h4>
          </div>
          <div className="col-lg-4 mt-5" style={{ alignContent: "left" }}>
            <table className="table table-hover mt-2">
              <thead style={{ border: "0px" }}>
                <tr style={{ border: "0px" }}>
                  <th style={{ border: "0px", textAlign: "center" }}>Local </th>
                  <th style={{ border: "0px", textAlign: "center" }}>
                    {" "}
                    Regional{" "}
                  </th>
                  <th style={{ border: "0px", textAlign: "center" }}>Global</th>
                  <th style={{ border: "0px", textAlign: "center" }}>
                    Engineering{" "}
                  </th>
                  <th style={{ border: "0px", textAlign: "center" }}>Total</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td style={{ textAlign: "center" }}>{LocalCount}</td>
                  <td style={{ textAlign: "center" }}>{RegionalCount}</td>
                  <td style={{ textAlign: "center" }}>{GlobalCount}</td>
                  <td style={{ textAlign: "center" }}>{EngineeringCount}</td>
                  <td style={{ textAlign: "center" }}>{TotalCount}</td>
                </tr>
              </tbody>
            </table>
          </div>
          <div className="col-lg-4 mt-5"></div>
        </div>
        <div className="row form-group p-0">
          <hr />
          <div className="row mt-2">
            <div className="col-lg-3">
              <h3 style={{ textAlign: "center" }}>SYSTEM CATEGORY</h3>
            </div>
            <div className="col-lg-2">
              <h3
                id="systemHeader"
                style={{ textAlign: "center", display: "none" }}
              >
                SYSTEMS
              </h3>
            </div>

            <div className="col-lg-5">
              <h3 style={{ textAlign: "center" }}>{AppNameHeader}</h3>
            </div>
            <div className="col-lg-2">
              <Link to="/AddPage" className="nav-link">
                <button type="button" id="Addbtn">
                  ADD
                </button>
              </Link>
            </div>
          </div>
          <div className="row ">
            <div className="col-lg-3">
              {systemCategory.map((category, index) => (
                <button
                  type="button"
                  id={"categoryBtn" + index}
                  className="btn btn-grey w-100 mt-2"
                  style={{ border: "0px", alignItems: "center" }}
                  onClick={() => this.onClickSystemCategory(category, index)}
                >
                  {category}
                </button>
              ))}
            </div>

            <div className="col-lg-2" id="rowid" style={{ overflowY: "auto" }}>
              {systemsForTableData.map((system, index) => (
                <button
                  type="button"
                  id={"SystemBtn" + index}
                  className="btn btn-grey w-100 mt-2"
                  style={{ border: "0px" }}
                  onClick={() => this.onClickSystem(system, index)}
                >
                  {system}
                </button>
              ))}
            </div>
            <div className="col-lg-7">
              <table
                className="table table-hover mt-2"
                id="table1"
                style={{ display: "none" }}
              >
                <thead>
                  <tr className="text-black">
                    <th
                      style={{
                        width: "100%",
                        height: "60px",
                        verticalAlign: "top",
                      }}
                    >
                      {purpose}
                    </th>
                  </tr>
                </thead>
              </table>

              <table
                className="table table-hover"
                id="table2"
                style={{ display: "none" }}
              >
                <thead style={{ height: "7rem" }}>
                  <tr className="text-black">
                    <td style={{ verticalAlign: "top" }}>
                      SYSTEM LNKS
                      <br />
                      <hr />
                      PRODUCTION LINK:{" "}
                      <a href={ProdUrl} target="_blank" className="text-black">
                        {ProdUrl.toLowerCase()}
                      </a>{" "}
                      <br />
                      <hr />
                      TEST LINK:{" "}
                      <a href={TestUrl} target="_blank" className="text-black">
                        {TestUrl.toLowerCase()}
                      </a>
                      <br />
                      <hr />
                      SUPPORT CONTACT
                      <br />
                      First Line support: {PrimaryMail} / {PrimaryPhone} /{" "}
                      {PrimayTeams}
                      <br />
                      Second Level Escalation: {SecondaryMail} /{" "}
                      {SecondaryPhone} / {SecondaryTeams}
                      <br />
                    </td>
                  </tr>
                </thead>
              </table>
              {/*<textarea id="textarea1" type="text" style={{ width: "100%", height: "250px", display: "none" }} value={purpose}></textarea>*/}
              {/*<textarea id="textarea2" style={{ width: "100%", height: "250px", display: "none" }}> </textarea>*/}
            </div>
          </div>
        </div>
        <div
          className="col-lg-12"
          style={{
            border: "1px solid black",
            position: "absolute",
            bottom: "0px",
            width: "100 %",
            textAlign: "center",
          }}
        >
          <h5 className="mt-2">
            System Development Department 1 <br /> Toyota Kirloskar Motors
            Pvt.Ltd{" "}
          </h5>
        </div>
        <div
          className="modal"
          data-bs-backdrop="static"
          data-bs-keyboard="false"
          id="ModalMessage"
        >
          <div
            className="modal-dialog modal-sm modal-dialog-centered"
            role="document"
          >
            <div className="modal-content">
              <div
                className="modal-header"
                style={{ backgroundColor: "#E2DED0" }}
              >
                <h5 className="modal-title">Website Information</h5>
                <button
                  type="button"
                  className="btn-close"
                  data-bs-dismiss="modal"
                  aria-label="Close"
                >
                  <span aria-hidden="true"></span>
                </button>
              </div>
              <div className="modal-body text-center">
                <div className="col-12">
                  <h5 className="text-danger text-center">
                    {this.state.purpose}
                  </h5>
                </div>
              </div>
              <div
                className="modal-footer p-1 m-1 d-flex justify-content-center"
                style={{ backgroundColor: "#E2DED0" }}
              >
                <h5 className="text-danger" id="message">
                  {this.state.Message}
                </h5>

                <button
                  type="button"
                  className="btn btn-grey"
                  onClick={() => this.onClickGotoWebsite()}
                  id="websiteBtn"
                >
                  Go to Website
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default withRouter(TkmAppDetails);
