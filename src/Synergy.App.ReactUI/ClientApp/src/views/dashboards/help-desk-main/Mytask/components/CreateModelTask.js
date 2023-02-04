import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Modal from "@mui/material/Modal";
import TaskForm from './TaskForm'

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
  Height: '2px',

}
const modalContentStyle = {
  position: "relative",
  background: "#fff",
  boxShadow: 20,
  mt: 2,
  width: "55rem",
  mb: 3,
  borderRadius: "5px",

};

export default function CreateModelTask() {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  return (
    <div>
      <Button onClick={handleOpen} sx={{ mr: 2, mb: 2, display: "flex", justifyContent: "space-between", height: "30px", width: "100px", gap: "8" }} variant="contained" >Create</Button>
      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>
            <TaskForm />
          </Box>
        </Box>
      </Modal>
    </div>
  );
}