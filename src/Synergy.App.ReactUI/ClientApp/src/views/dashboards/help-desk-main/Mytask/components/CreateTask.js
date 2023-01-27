import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import { width } from "@mui/system";



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
  mt:1,
  width:"50rem",
  mb:1,
  borderRadius:"5px",
};

export default function BasicModal() {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  return (
    <div>
      <Button sx={{height : "30px"}} onClick={handleOpen} variant='contained'>
        INFORMATION</Button>
      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>
          <Typography id="modal-modal-title" variant="h4" component="h2">
            <center>XTRANET INDIA</center>
          </Typography>
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
          Synergy refers to when an interaction of elements produces an effect that is greater than the effect that would have resulted from simply adding up the effects of each individual element.<br/><br/>
          
          When you combine things—chemicals, ingredients, people—you often expect these things to interact in a certain way based on what has been included. But when something extra happens, something greater, this is synergy. Synergy implies that the magic is in the combination, as opposed to in the individual elements themselves.
            
          </Typography>
        </Box>
        </Box>
      </Modal>
    </div>

  );
}