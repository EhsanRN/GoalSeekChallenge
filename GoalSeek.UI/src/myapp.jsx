import { useState } from 'react';
import './App.css';

 function MyApp() {
     const [responseData, setResponseData] = useState();
     const [responseError, setResponseError] = useState();
     function handleSubmit(e) {
         // Read the form data
         const form = e.target;
         const formData = new FormData(form);

         e.preventDefault();
         fetch("/api/goalseek", {
             method: "post",
             headers: {
                 'Accept': 'application/json',
                 'Content-Type': 'application/json'
             },

             //make sure to serialize your JSON body
             body: JSON.stringify({
                 formula: formData.get('Formula')+ " * Input",
                 input: formData.get('Input'),
                 targetResult: formData.get('TargetResult'),
                 maximumIterations: formData.get('MaximumIterations')
             })
         }).then((response) => response.json())
             .then((json) => setResponseData(json))


     }

         return (
             <div>
                 <h1>GOALSEEK APP</h1>
                 <h3>Please enter values in to the form below:</h3>
                 <div style={{ margin: "0 auto", width:"50%" }}>
                     <form onSubmit={handleSubmit}>
                         <label>Formula:
                             <span style={{ width: "230px", float: "left" }}><input type="text" name="Formula" /> <span>* Input</span></span>
                         </label>
                         <label style={{ float: "left" }}>Input
                             <input type="text" name="Input" />
                         </label>
                         <label>Target Result
                             <input type="text" name="TargetResult" />
                         </label>
                         <label>Maximum Iterations
                             <input type="text" name="MaximumIterations" />
                         </label>
                         <label style={{ marginTop:"30px"}}>
                             <input type="submit" value="Submit" />
                         </label>
                     </form>
                     {/*{responseData && responseData.targetInput ? (<p>Target Input: {responseData.targetInput}</p>) : ("")}*/}
                     {/*{responseData && responseData.maximumIterations ? (<p>Maximum Iterations: {responseData.maximumIterations}</p>) : ("")}*/}

                 </div>
                 <div style={{ marginTop: "60px" } }>Json Result: {JSON.stringify(responseData, null, 4)}</div>
                 {/*<div>{responseError}</div>*/}

             </div>
         );
}
export default MyApp;
