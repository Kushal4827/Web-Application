import React, { Component } from "react";
import "./App.css";
import "./Assests/css/bootstrap.css";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import TkmAppDetails from "./TkmAppDetailsComponent/TkmAppDetails";
import AddPage from "./TkmAppDetailsComponent/AddPage";
import { ToastContainer } from "react-toastify";
import "../node_modules/react-toastify/dist/ReactToastify.css";

class App extends Component {
  render() {
    return (
      <div>
        <ToastContainer />
        <BrowserRouter>
          <Switch>
            <Route path="/" component={TkmAppDetails} exact />
            <Route path="/AddPage" component={AddPage} />
          </Switch>
        </BrowserRouter>
      </div>
    );
  }
}
export default App;
