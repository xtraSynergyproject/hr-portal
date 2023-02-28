import * as React from "react";
import { Grid, Box } from "@mui/material";
import Button from "@mui/material/Button";
import Icon from 'src/@core/components/icon';
import Divider from '@mui/material/Divider';
import Modal from "@mui/material/Modal";
import { useState } from 'react';
import TextField from '@mui/material/TextField';
import Backdrop from '@mui/material/Backdrop'
import CircularProgress from '@mui/material/CircularProgress'




import MoreHorizIcon from '@mui/icons-material/MoreHoriz';
import Card from '@mui/material/Card'
import IconButton from '@mui/material/IconButton'
import CardContent from '@mui/material/CardContent'
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import HdrAutoIcon from '@mui/icons-material/HdrAuto';
// import RestartAltIcon from '@mui/icons-material/RestartAlt';



import PersonIcon from '@mui/icons-material/Person';






const modalWrapper = {
  overflow: 'auto',
  maxHeight: '100vh',
  display: 'flex'
}

const modalBlock = {
  position: 'relative',
  zIndex: 0,
  display: 'column',
  alignItems: 'center',
  justifyContent: 'center',
  marginLeft:'63rem',
  marginTop:'12rem'
}

const modalContentStyle = {
  position: 'relative',
  background: '#fff',
  boxShadow: 24,
  mt: 3,
  width: '12rem',
  mb: 3,
  borderRadius: '10px'
}



export default function BasicModal() {

  const [open, setOpen] = React.useState(false);

  const handleOpen = () => setOpen(true);

  const handleClose = () => setOpen(false);

  const [value, setValue] = useState('1')
  const [reload, setReload] = useState(false)

  const handleBackDrop = () => {
    setReload(true)
    setTimeout(() => {
      setReload(false)
    }, 2000)
  }

  const handleChange = (event, newValue) => {
    setValue(newValue)

  }


return (

<>
{/* <Fade in={visibility} timeout={300}> */}

<Card>
      <Box size='small' sx={{cursor:'pointer'}}onClick={handleOpen}>
        < MoreHorizIcon/>

    </Box>




      <Modal

        open={open}

        sx={modalWrapper}

        onClose={handleClose}

        aria-labelledby="modal-modal-title"

        aria-describedby="modal-modal-description"

      >



      <Box sx={modalBlock}>

      <Box sx={modalContentStyle}>

        {/* <CardHeader
          title='Remove Card'
          action={
            <IconButton
              size='small'
              aria-label='collapse'
              sx={{ color: 'text.secondary' }}
              onClick={() => handleClose(false)}
            >
              <Icon icon='mdi:close' fontSize={20} />
            </IconButton>
          }
        /> */}
        <CardContent>
            <Box component='span' sx={{ display:'column' }}>
              <Box  sx={{display:'flex'}}>
             <CalendarMonthIcon/>             
              Sortby Date

              </Box>
              <Box sx={{display:'flex'}}>
              <HdrAutoIcon />
              Sortby Subject
              </Box>
              <Box sx={{display:'flex'}}>
              <PersonIcon/>
              Sortby Owner


              </Box>
              {/* for page reload */}
              <Box sx={{display:'flex'}}>
               {/* <RestartAltIcon/> */}
            
            <IconButton
            size='small'
            label='Reset'
            sx={{ color: 'text.secondary' }}
            onClick={() => handleBackDrop()}
          >
            <Icon icon='mdi:refresh' fontSize={20} />
            Reset
          </IconButton>
        
              {/* Reset */}


              </Box>


            </Box>
        </CardContent>
        <Backdrop
        open={reload}
        sx={{
          position: 'absolute',
          color: 'common.white',
          zIndex: theme => theme.zIndex.mobileStepper - 1
        }}
      >
        <CircularProgress color='inherit' />
      </Backdrop>
     </Box> 


</Box>

</Modal >
      </Card>
    {/* </Fade> */}



       
    </>

  );

}

