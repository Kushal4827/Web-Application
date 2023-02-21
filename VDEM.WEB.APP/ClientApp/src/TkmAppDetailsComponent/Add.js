// constructor(props) {
//   super(props);

//   this.state = {
// id: '',
//  newValue: ''}
//
//   };

//   this.handleInputChange = this.handleInputChange.bind(this);
//   this.handleSubmit = this.handleSubmit.bind(this);
// }

// handleInputChange(event) {
//   const target = event.target;
//   const value = target.value;
//   const name = target.name;

//   this.setState({
//     [name]: value
//   });
// }

// handleSubmit(event) {
//   event.preventDefault();

//   fetch('/update-data.php', {
//     method: 'POST',
//     headers: {
//       'Content-Type': 'application/json'
//     },
//     body: JSON.stringify(this.state)
//   })
//   .then(function(response) {
//     console.log(response);
//   })
//   .catch(function(error) {
//     console.log(error);
//   });
// }

// render() {
//   return (
//     <form onSubmit={this.handleSubmit}>
//       <label>
//         ID:
//         <input type="text" name="id" value={this.state.id} onChange={this.handleInputChange} />
//       </label>
//       <br />
//       <label>
//         New Value:
//         <input type="text" name="newValue" value={this.state.newValue} onChange={this.handleInputChange} />
//       </label>
//       <br />
//       <button type="submit">Submit</button>
//     </form>
//   );
// }
