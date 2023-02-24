import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Icon from 'src/@core/components/icon'
import Modal from "@mui/material/Modal";
import Divider from "@mui/material/Divider";
import PayrollAddEmployeeForm from "./PayrollAddEmployeeForm";




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

  width:"60rem",

  mb:3,

  borderRadius:"10px",

}; 


export default function BasicModal() {

  const [open, setOpen] = React.useState(false);

  const handleOpen = () => setOpen(true);

  const handleClose = () => setOpen(false);


  return (

    <div>

<Button  onClick={handleOpen} sx={{ width: '170px', mr: 2 }} variant='contained'>Add Employee
      </Button>

      <Modal

        open={open}

        sx={modalWrapper}

        onClose={handleClose}

        aria-labelledby="modal-modal-title"

        aria-describedby="modal-modal-description"

      >

        <Box sx={modalBlock}>

          <Box sx={modalContentStyle}>

          <Box sx={{ mb: "10px", display: 'flex', justifyContent: 'space-between', alignItems:"center" }} className='demo-space-x'>
              <Typography  sx={{ pl: 4 }} variant='h5' component='h3'>
                Add Employee
              </Typography>

              <Box >
               <Typography
                  sx={{
                    borderRadius: '50px',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    fontSize: '13px',
                    cursor:"pointer"
                  }}
                  onClick={handleClose}
                  component='label'
                >
                  <Icon icon='mdi:close' fontSize='18px' />
                  Close
                </Typography>
              </Box>
            </Box>
            <Divider />
          <PayrollAddEmployeeForm />

        </Box>

        </Box>

      </Modal>

    </div>

  );

}