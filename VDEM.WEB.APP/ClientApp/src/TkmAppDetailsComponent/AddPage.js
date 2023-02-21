import React, { Component } from "react";
import logo1 from "../Assests/img/Toyota-Logo.png";
import * as vdemConst from "../VdemConstants";
import { withRouter } from "react-router-dom";
import toastComponent from "./toastcomponent";

class AddPage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      AddAppDetailsIP: [],
    };
    this.handleInputChange = this.handleInputChange.bind(this);
  }

  handleInputChange = (event) => {
    event.preventDefault();
    let Syscategory = event.target.syscategory.value;
    let System = event.target.system.value;
    let Prodlink = event.target.prodlink.value;
    let Testlink = event.target.testlink.value;
    let Purpose = event.target.purpose.value;
    let Psemail = event.target.psemail.value;
    let Psphone = event.target.psphone.value;
    let Psteams = event.target.psteams.value;
    let Ssemail = event.target.ssemail.value;
    let Ssphone = event.target.ssphone.value;
    let Ssteams = event.target.ssteams.value;

    this.setState(
      (prevState) => ({
        AddAppDetailsIP: {
          ...prevState.AddAppDetailsIP,
          AddAppDetails: {
            ...prevState.AddAppDetailsIP.AddAppDetails,

            APPLICATION_NAME: System,
            APPLIATION_CATEGORY: Syscategory,
            PRODUCTION_URL: Prodlink,
            TEST_URL: Testlink,
            PURPOSE: Purpose,
            PRIMARY_SUPPORT_EMAIL: Psemail,
            PRIMARY_SUPPORT_PHONE: Psphone,
            PRIMARY_SUPPORT_TEAMS: Psteams,
            SECONDARY_SUPPORT_EMAIL: Ssemail,
            SECONDARY_SUPPORT_PHONE: Ssphone,
            SECONDARY_SUPPORT_TEAMS: Ssteams,
          },
        },
      }),
      () => {
        this.CallAddApi();
      }
    );
  };

  CallAddApi() {
    console.log(5);
    var JsonBody = this.state.AddAppDetailsIP;
    console.log(JsonBody);
    var Token = sessionStorage.getItem("Token");
    fetch(vdemConst.WEB_API_URL + "/TkmAppDetails/AddAppDetails", {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
        Authorization: "Bearer " + Token,
      },
      body: JSON.stringify(JsonBody),
    }).then(
      (data) => {
        if (data.returnValue < 0) {
          toastComponent("error", "Failed, Contact Administrator");
          return;
        }
        toastComponent("success", "Added Successfuly", this.Reload);
      },
      (error) => {
        toastComponent(
          "error",
          "Session Expired ! please Re-Login",
          this.SessionTimeOut
        );
      }
    );
  }
  Reload() {
    window.location.reload();
  }

  Rollback() {
    window.location.href = "/LineMaster";
  }

  SessionTimeOut() {
    sessionStorage.clear();
    window.location.href = "/";
  }

  render() {
    return (
      <>
        <div
          className="row w-100"
          style={{
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

          <hr
            style={{
              background: "black",
              height: "2px",
              border: "none",
            }}
          />
          <div className="col-lg-12">
            <h5 className="mt-2">Add TKM App Details</h5>
          </div>
        </div>

        <form onSubmit={this.handleInputChange}>
          <div className="row">
            <div className="col-lg-4 mt-5">
              <div className="form-row">
                <div className="row">
                  <label htmlFor="syscategory">
                    System Category :<b style={{ color: "red" }}>*</b>
                  </label>
                  <select id="syscategory" className="form-control" required>
                    <option disabled selected value>
                      ---Select---
                    </option>
                    <option>Local</option>
                    <option>Regional</option>
                    <option>Global</option>
                    <option>Engineering system</option>
                  </select>
                </div>
                <br />
                <div className="row">
                  <label htmlFor="system">
                    System : <b style={{ color: "red" }}>*</b>
                  </label>
                  <input
                    type="text"
                    id="system"
                    className="form-control"
                    placeholder="System"
                    required
                  />
                </div>
                <br />

                <div className="col">
                  <label htmlFor="prodlink">Production Link :</label>
                  <input
                    type="text"
                    id="prodlink"
                    className="form-control"
                    placeholder="Production Link"
                  />
                </div>
                <br />
                <div className="col">
                  <label htmlFor="testlink">
                    Purpose : <b style={{ color: "red" }}>*</b>
                  </label>
                  <input
                    type="text"
                    id="purpose"
                    className="form-control"
                    placeholder="Purpose"
                    required
                  />
                </div>
                <br />
              </div>
            </div>
            <div className="col-lg-4 mt-5">
              <div className="form-row">
                <div className="col">
                  <label htmlFor="testlink">Test Link :</label>
                  <input
                    type="text"
                    id="testlink"
                    className="form-control"
                    placeholder="Test Link"
                  />
                </div>
                <br />
                <div className="col">
                  <label htmlFor="syscategory">
                    Primary support Email : <b style={{ color: "red" }}>*</b>
                  </label>
                  <input
                    type="text"
                    id="psemail"
                    className="form-control"
                    placeholder="Primary support Email "
                    required
                  />
                </div>
                <br />
                <div className="col">
                  <label htmlFor="system">
                    Primary support phone : <b style={{ color: "red" }}>*</b>{" "}
                  </label>
                  <input
                    type="text"
                    id="psphone"
                    className="form-control"
                    placeholder="Primary support phone"
                    required
                  />
                </div>
                <br />
                <div className="col">
                  <label htmlFor="syslink">Primary support Teams :</label>
                  <input
                    type="text"
                    id="psteams"
                    className="form-control"
                    placeholder="Primary support Teams"
                  />
                </div>
                <br />
              </div>
            </div>
            <div className="col-lg-4 mt-5">
              <div className="form-row">
                <div className="col">
                  <label htmlFor="prodlink"> Secondary support Email:</label>
                  <input
                    type="text"
                    id="ssemail"
                    className="form-control"
                    placeholder="Secondary support Email"
                  />
                </div>
                <br />
                <div className="col">
                  <label htmlFor="testlink">Secondary support Phone:</label>
                  <input
                    type="text"
                    id="ssphone"
                    className="form-control"
                    placeholder="Secondary support Phone"
                  />
                </div>
              </div>
              <br />
              <div className="col">
                <label htmlFor="testlink">Secondary support Teams:</label>
                <input
                  type="text"
                  id="ssteams"
                  className="form-control"
                  placeholder="Secondary support Teams"
                />
              </div>
              <br />
            </div>

            <div className="row"></div>
            <div className="col-md-2 mb-4"></div>
            <div className="col-md-2 mb-4">
              <button
                className="btn btn-grey"
                onClick={this.props.history.goBack}
              >
                Back
              </button>
            </div>
            <div className="col-md-4 mb-4"></div>
            <div className="col-md-4 mb-4">
              <button type="submit" className="btn btn-grey">
                Submit
              </button>
            </div>
          </div>
        </form>
      </>
    );
  }
}

export default withRouter(AddPage);
