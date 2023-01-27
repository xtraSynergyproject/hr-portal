import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";


 
 import Form from './Form'





const modalWrapper = {
  overflow: "auto",
  maxHeight: "100vh",
  display: "flex",

};

const modalBlock = {
  position: "relative",
  zIndex: 0,
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  margin: "auto",
  Height:'2px',

}

const modalContentStyle = {
  position: "relative",
  background: "#fff",
  boxShadow: 40,
  mt: 2,
  width: "55rem",
  mb: 3,
  borderRadius: "5px",
  

};

export default function BasicModal() {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  return (
    <div>

      <Button onClick={handleOpen} sx={{ mr: 4, mb: 2 ,margin:"10px" }}  variant="contained" >Create</Button>
       

      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>

            
            <Form />



          </Box>
        </Box>
      </Modal>


      









    </div>
  );
}