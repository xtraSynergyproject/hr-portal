import React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import CardLeave from './CardLeave'


//import Box from '@mui/material/Box';





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
  // background:"#fff",
  boxShadow:20,
  mt:2,
  // width:"30rem",
  mb:3,
  borderRadius:"10px",
  
};

export default function BasicModal() {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  return (
    <div>
     
      <Button sx={{color:'#fff'}} onClick={handleOpen}>New Leave Request</Button>
      
      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>

          <Box sx={{ display: 'flex' }}>
          <CardLeave/>


          
         
         
      
    </Box>


          
  


         
          
          
          
        </Box>
        </Box>
      </Modal>
    </div>
  );
}