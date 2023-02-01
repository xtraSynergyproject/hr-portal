import React from "react";
import { Button } from "@mui/material";
import FormControlLabel from "@mui/material/FormControlLabel";
import Checkbox from "@mui/material/Checkbox";

function SubButton() {
  return (
    <div>
      <span>
        <FormControlLabel sx={{marginLeft : "4.2%"}} 
          control={<Checkbox required/>}
          label="Agree our terms and conditions"
        />
      </span>
      <span>
        <Button
          type="submit"
          variant="contained"
          sx={{
            width: "150px",
            height: "50px",
            float: "right",
            marginBottom: "1.5%",
            marginRight: "4.2%",
          }}
        >
          Submit
        </Button>
      </span>
    </div>
  );
}

export default SubButton;
