import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import ContactIcon from './ContactIcon'
//  import Form from './Form';
import InputForm from './InputForm'





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
  boxShadow: 20,
  mt: 2,
  width: "45rem",
  mb: 3,
  borderRadius: "5px",

};

export default function BasicModal() {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  return (
    <div>

      <Button onClick={handleOpen}><ContactIcon /></Button>

      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>

            {/* <Form/> */}
            <InputForm />



          </Box>
        </Box>
      </Modal>
    </div>
  );
}