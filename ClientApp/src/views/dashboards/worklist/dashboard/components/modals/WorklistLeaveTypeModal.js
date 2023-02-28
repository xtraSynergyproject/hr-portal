import * as React from "react";

import Box from "@mui/material/Box";

import Button from "@mui/material/Button";

import Typography from "@mui/material/Typography";

import Modal from "@mui/material/Modal";
import { Divider } from "@mui/material";







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

  width:"20rem",

  mb:3,

  borderRadius:"10px",

};



export default function BasicModal() {

  const [open, setOpen] = React.useState(false);

  const handleOpen = () => setOpen(true);

  const handleClose = () => setOpen(false);



  return (

    <div>

      <Button onClick={handleOpen}>+Create Service</Button>

      <Modal

        open={open}

        sx={modalWrapper}

        onClose={handleClose}

        aria-labelledby="modal-modal-title"

        aria-describedby="modal-modal-description"

      >

        <Box sx={modalBlock}>

          <Box sx={modalContentStyle}>

          <Typography id="modal-modal-title" variant="h6" component="h2"></Typography>
        </Box>
        <Divider/>

        </Box>

      </Modal>

    </div>

  );

}


