import * as React from "react";

import Box from "@mui/material/Box";

import Button from "@mui/material/Button";

import Typography from "@mui/material/Typography";

import Modal from "@mui/material/Modal";
import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton';
import Icon from 'src/@core/components/icon';
import DialogTitle from '@mui/material/DialogTitle';
import ModalUserInfo from "./ModalUserInfo";
import DescriptionModal from "./DescriptionModal";
import TextField from '@mui/material/TextField';






const modalWrapper = {

  overflow: "auto",

  //maxHeight: "2000vh",

  display: "flex",

};



const modalBlock = {

  position: "relative",

  zIndex: 0,

  display: "flex",

  alignItems: "center",

  justifyContent: "center",

  margin: "auto",

}



const modalContentStyle = {

  position: "relative",

  background: "#fff",

  boxShadow: 24,


  width: "60rem",
  
  cursor:"pointer",

  borderRadius: "10px",

};



export default function BasicModal() {

  const [open, setOpen] = React.useState(false);

  const handleOpen = () => setOpen(true);

  const handleClose = () => setOpen(false);





  return (

    <div>


      <Button variant='contained' sx={{ mt: 13, ml: 10, textTransform:'capitalize'}} onClick={handleOpen}>+Create</Button>

      <Modal

        open={open}

        sx={modalWrapper}

        onClose={handleClose}

        aria-labelledby="modal-modal-title"

        aria-describedby="modal-modal-description"

      >

        <Box sx={modalBlock}>

          <Box sx={modalContentStyle}>

            <Box>

              <DialogTitle>Create Task</DialogTitle>
              <IconButton
                aria-label='close'
                onClick={handleClose}
                sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.500' }}
              >
                <Icon icon='mdi:close' />
              </IconButton>
              <Divider />
            </Box>
            <Box sx={{ backgroundColor: '#f0f0f0' }}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                <Box>
                  <Typography variant='h6' component='h6' sx={{alignText:'center' }}>GeneralEmailTask</Typography>
                </Box>
                <Box sx={{ display: 'flex', justifyContent: 'space-around' }}>
                  <Typography
                    sx={{
                      borderRadius: '50px',
                      display: 'flex',
                      flexDirection: 'column',
                      alignItems: 'center',
                      fontSize: '13px',
                      cursor: "pointer"
                    }}
                    onClick={handleClose}
                    component='label'
                  >
                    <Icon icon='mdi:pencil' fontSize='18px' />
                    Action
                  </Typography>
                  <Typography
                    sx={{
                      borderRadius: '50px',
                      display: 'flex',
                      flexDirection: 'column',
                      alignItems: 'center',
                      fontSize: '13px',
                      cursor: "pointer"
                    }}
                    component='label'
                  >
                    <Icon icon='mdi:attachment' fontSize='18px' />
                    {/* <AttachFileOutlinedIcon/> Attachment */}
                    <input type='file' hidden />
                  </Typography>
                  <Typography
                    sx={{
                      borderRadius: '50px',
                      display: 'flex',
                      flexDirection: 'column',
                      alignItems: 'center',
                      fontSize: '13px',
                      cursor: "pointer"
                    }}
                    onClick={handleClose}
                    component='label'
                  >
                    <Icon icon='mdi:more' fontSize='18px' />
                    More Option
                  </Typography>


                  {/* <Typography
                    sx={{
                      borderRadius: '50px',
                      display: 'flex',
                      flexDirection: 'column',
                      alignItems: 'center',
                      fontSize: '13px',
                      cursor: "pointer"
                    }}
                    onClick={handleClose}
                    component='label'
                  >
                    <Icon icon='mdi:close' fontSize='18px' />
                    close
                  </Typography> */}
                </Box>

              </Box>
              <Divider />
              <ModalUserInfo />
              <Box sx={{ ml: 4}}>
                Subject
                {/* <Box component="form" sx={{ border: '1px solid grey', borderRadius:1,width: '58rem', height:40 }}> */}
                <Box
                    component="form"
                    sx={{
                        '& .MuiTextField-root': { m: 1, width: '57rem',bgcolor: 'background.paper',
                        color: (theme) => theme.palette.getContrastText(theme.palette.background.paper), },
                    }}
                    noValidate
                    autoComplete="off"
                >
                    <div>
                        <TextField
                            // label="Size"
                            id="outlined-size-small"
                            // defaultValue="Small"
                            size="small"
                        />
                    </div>

                </Box>

            </Box>
              <DescriptionModal />
              {/* <Box>
                <Box>
                  Assign To
                </Box>
                <Box>
                  <TextField
                    id="filled-select-currency"
                    // select
                    // label="Select"
                    defaultValue="EUR"
                    helperText="Please select your currency"
                    variant="filled"
                  />
                </Box>


              </Box> */}



            </Box>
          </Box>

        </Box>

      </Modal>

    </div>
    

  );

}

