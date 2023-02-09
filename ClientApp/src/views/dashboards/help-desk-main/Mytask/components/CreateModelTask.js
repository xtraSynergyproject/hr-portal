import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Modal from "@mui/material/Modal";
import CreateModelTask from "./CreateModelTask";
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
  mt: 0,
  width: "45rem",
  mb: 0,
  borderRadius: "5px",
};

export default function RequestCreate() {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  return (

    <div>
      <Button onClick={handleOpen}>+ Add Task</Button>
      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}

      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>

            <form>
              <Box sx={{ m: 2, display: "flex", justifyContent: "space-between", paddingLeft: '10px' }}>
                <h1>Create Home</h1>
              </Box>
              <Box sx={{ display: 'flex', }}>
                <Box sx={{ margin: '10px' }}>
                  <Button variant="contained" size="medium">
                    ALL
                  </Button>
                </Box>
                <Box>
                  <CreateModelTask />
                </Box>
              </Box>
            </form>
          </Box>
        </Box>
      </Modal>
    </div>

  );
}