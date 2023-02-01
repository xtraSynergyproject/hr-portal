import * as React from 'react';
import Box from '@mui/material/Box';
import Modal from '@mui/material/Modal';
import Button from '@mui/material/Button';
import AddIcon from '@mui/icons-material/Add';
import CancelIcon from '@mui/icons-material/Cancel';
import FilePresentIcon from '@mui/icons-material/FilePresent';
import StyleIcon from '@mui/icons-material/Style';
const style = {
  position: 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 400,
  bgcolor: 'background.paper',
  border: '2px solid #000',
  boxShadow: 24,
  pt: 2,
  px: 4,
  pb: 3,
};

function ChildModal() {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => {
    setOpen(true);
  };
  const handleClose = () => {
    setOpen(false);
  };

  return (
    <React.Fragment>
      <Button onClick={handleOpen}>Open Child Modal</Button>
      <Modal
        hideBackdrop
        open={open}
        onClose={handleClose}
        aria-labelledby="child-modal-title"
        aria-describedby="child-modal-description"
      >
        <Box sx={{ ...style, width: 200 }}>
          <h2 id="child-modal-title">Text in a child modal</h2>
          <p id="child-modal-description">
            Lorem ipsum, dolor sit amet consectetur adipisicing elit.
          </p>
          <Button onClick={handleClose}>Close Child Modal</Button>
        </Box>
      </Modal>
    </React.Fragment>
  );
}

export default function NestedModal() {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => {
    setOpen(true);
  };
  const handleClose = () => {
    setOpen(false);
  };

  return (
    <div>
        <h2>HR Location</h2>
        <div>
      <Button onClick={handleOpen}><AddIcon />Create</Button>
      </div>
      {/* <div>
        <button>Import/Export</button>
      </div> */}
      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="parent-modal-title"
        aria-describedby="parent-modal-description"
      >
        <Box sx={{ ...style, width: 400 }}>
            <div style={{backgroundImage:"https://cdn.pixabay.com/photo/2017/06/14/08/20/map-of-the-world-2401458_960_720.jpg",background:"cover"}}>
          <h2 id="parent-modal-title">Location</h2>
          </div>
          <div style={{display:"flex",justifyContent:"space-between"}}>
           <div>
           <CancelIcon/>
            <p>Close</p>
            
           </div>
           <div>
            <p>N-26-12-2022-57</p>
            <p>Note No</p>
           </div>
           <div>
            <p>Draft</p>
            <p>Status</p>
           </div>
           <div>
            <p>1</p>
            <p>Version No</p>
           </div>
           <div>
           <FilePresentIcon/>
            <span>0</span>
            <p>Attachment</p>
            {handleOpen}
           </div>
           <div>
          <StyleIcon/>
            <p>Tags</p>
          </div>
          <div>
          <StyleIcon/>
            <p>Log</p>
          </div>
          </div>
         
          <ChildModal />
        </Box>
      </Modal>
    </div>
  );
}