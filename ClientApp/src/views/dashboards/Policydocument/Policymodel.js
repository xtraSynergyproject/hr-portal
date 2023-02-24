import * as React from 'react';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Modal from '@mui/material/Modal';
import Pdfmodule from './Pdfmodule'
import CancelIcon from '@mui/icons-material/Cancel'


const style = {
  position: 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  // width: 400,
  bgcolor: 'background.paper',
  // border: '2px solid #000',
  boxShadow: 24,
  p: 4,
};

export default function BasicModal() {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);


  const onButtonClick = () => {
    // using Java Script method to get PDF file
    fetch('SamplePDF.pdf').then(response => {
        response.blob().then(blob => {
            // Creating new object of PDF file
            const fileURL = window.URL.createObjectURL(blob);
            // Setting various property values
            let alink = document.createElement('a');
            alink.href = fileURL;
            alink.download = 'one.pdf';
            alink.click();
        })
    })
  }
  return (
    <div>
      <Button onClick={handleOpen}>View</Button>
      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={style}>
        <CancelIcon onClick ={handleClose} sx={{float:"right",mt:1}}/>
          <Typography id="modal-modal-title" variant="h6" component="h2">
          <Button onClick={onButtonClick}>Download</Button>
          </Typography>
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
      <Pdfmodule/>

          </Typography>
         
        </Box>
        
      </Modal>
    </div>
  );
}