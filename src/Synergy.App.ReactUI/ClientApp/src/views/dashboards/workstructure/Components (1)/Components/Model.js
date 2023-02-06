import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import Abc from 'src/views/dashboards/workstructure/Components (1)/Abc'
// import NehaPagal from "./NehaPagal";
import Xyz from 'src/views/dashboards/workstructure/Components (1)/Components/Xyz'
// import Form from 'src/views/dashboards/workstructure/Components (1)Components/Form'
import Form from './Form'
import CancelIcon from '@mui/icons-material/Cancel'



// import Tabs from 'sr/'

// import StepperLinearWithValidation from 'src/views/forms/form-wizard/StepperVerticalWithoutNumbers'

const modalWrapper = {
  overflow:"auto",
  maxHeight:"100vh",
  display:"flex",
};

const modalBlock = {
  position:"relative",
  zIndex:0,
  display:"flex",
  alignItems:"center",
  justifyContent:"center",
  margin:"auto",
}

const modalContentStyle ={
  position:"relative",
  background:"#fff",
  boxShadow:24,
  mt:3,
  width:"100%",
  height:"100%",
  mb:3,
  borderRadius:"10px",
};

export default function Model() {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  return (
    <div>
      <Button variant="contained" onClick={handleOpen}sx={{ml:70}}>Create</Button>
      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>

  <CancelIcon onClick ={handleClose} sx={{float:"right",mt:1}}/>

<Abc/>

<Xyz/>
<Form/>
        </Box>
        </Box>
      </Modal>
    </div>
  );
}
