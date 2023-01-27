import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Modal from "@mui/material/Modal";
import SearchBar from "./SearchBar";
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
  width: "45rem",
  mb: 3,
  borderRadius: "5px",

};

export default function BasicModal() {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  return (

    <Box>
      {<Box onClick={handleOpen} variant='contained'>ADD Request </Box>}
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
            <form>
              <Box sx={{ m: 2, display: "flex", justifyContent: "space-between", width: "700px" }}></Box>
              <Box>
                <div>
                  <h1>Create Task</h1>

                </div>
                <Button variant='contained'>
                  ALL
                </Button>
              </Box>
              <Box>
                <SearchBar />
              </Box>
            </form>
          </Box>
        </Box>
      </Modal>
    </Box>
  );
}