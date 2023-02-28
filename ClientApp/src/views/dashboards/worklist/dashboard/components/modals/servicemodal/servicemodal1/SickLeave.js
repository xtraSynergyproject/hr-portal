import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import TextField from '@mui/material/TextField'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
// import MenuBtn from './MenuBtn'
import { Card, Divider } from '@mui/material'

import Avatar from '@mui/material/Avatar';
import ActionButton from './ActionButton';
import ModalUserInfo from '../../modals1/ModalUserInfo'
// import Fab from '@mui/material/Fab';
// import EditIcon from '@mui/icons-material/Edit';
// import { AttachFile } from '@mui/icons-material';
// import CloseIcon from '@mui/icons-material/Close';
// import MoreHorizIcon from '@mui/icons-material/MoreHoriz';
// import EmailIcon from '@mui/icons-material/Email';



const modalWrapper = {
  overflow: 'auto',
  maxHeight: '100vh',
  display: 'flex'
}

const modalBlock = {
  position: 'relative',
  zIndex: 0,
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  margin: 'auto'
}

const modalContentStyle = {
  position: 'relative',
  background: '#fff',
  boxShadow: 24,
  mt: 3,
  width: '60rem',
  mb: 3,
  borderRadius: '10px'
}

function SickLeave() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <Box>
      <Button variant='contained' size='small'  sx={{width:'10rem',mt:1,textTransform:'capitalize',fontSize:12,P:1}} onClick={handleOpen}>
        SickLeave
      </Button>

      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby='modal-modal-title'
        aria-describedby='modal-modal-description'
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>
            <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between' }} className='demo-space-x'>
              <Typography sx={{ p: 4 }} variant='h5' component='h3'>
                CreateService 
              </Typography>

              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignContent: 'center' }}>
            {/*   <Button sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }} component='label'>
                  <Icon icon='mdi:attachment-plus' />
                  Attachment
                  <input type='file' hidden />
                </Button>

                <MenuBtn /> */}

                <Button
                  sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }}
                  onClick={handleClose}
                  component='label'
                >
                  <Icon icon='mdi:close' />
                  Close
                </Button>
              </Box>
            </Box>
            <Divider />
            <Box sx={{display:'column'}}>
              <Box
                sx={{
                  
                  ml:10,
                  width: '15rem',
                  height: '7vh',
                  backgroundColor: '#346cb0',
                  borderRadius: 10,
                }}
                
              >
              <Box sx={{display:'flex',ml:7}}>
              <Avatar alt="Travis Howard" image="https://media.istockphoto.com/id/1368424494/photo/studio-portrait-of-a-cheerful-woman.jpg?s=1024x1024&w=is&k=20&c=9eszAhNKMRzMHVp4wlmFRak8YyH3rAU8re9HjKA6h3A=" />
               <Typography sx={{ml:5, color:'#fff'}}>UnAbsent Laeave</Typography>
               </Box>
               </Box>


           

              <Box>
              <Box sx={{display:'flex'}}>
               
                <Box>  
                  <Typography variant='h6' component='h2' sx={{ml:5,mt:7}}>
                  Sick Leave
                </Typography>
                </Box>
                
                  <Box sx={{ml:'60%'}}>
                  <ActionButton/>

                    </Box> 


                  
                <Divider />
              </Box>
                <Box sx={{ display: 'column', mt:10,ml:2, justifyContent: 'flex-end', gap: 3 }}>
                 <ModalUserInfo/>
                 <Box sx={{ml:5}}>
                 <Button variant='contained' size='medium'>Save As Draft</Button>
                 <Button variant='contained' size='medium' sx={{ml:2}}>Submit</Button>

                 </Box>
                 <Divider/>
                 </Box>
                 <TextField
                      required
                      size='small'
                      sx={{ marginBottom: '20px',width:'57rem',ml:4 }}
                      id='date'
                      label='Leave Start Date*'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />
                    <TextField
                      required
                      size='small'
                      sx={{ marginBottom: '20px',width:'57rem',ml:4 }}
                      id='date'
                      label='Leave End Date*'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />

                 
                <TextField
                  id='outlined-basic'
                  label='Leave Duration(Calendar Days)'
                  size='small'
                  variant='outlined'
                  sx={{ mt: 3, mb:4,ml:4,width:'57rem' }}
                />
                <TextField
                  id='outlined-basic'
                  label=' Leave Duration( Working Days)'
                  size="small"
                  // multiline
                  // rows={4}
                  variant='outlined'
                  sx={{ mt: 1.5, mb: 5, ml:4,width:'57rem' }}
                />
                <TextField
                  id='outlined-basic'
                  label='Telephone No'
                  size="small"
                  // multiline
                  // rows={4}
                  variant='outlined'
                  sx={{ mt: 1.5, mb: 5, ml:4,width:'57rem' }}
                />
                <TextField
                  id='outlined-basic'
                  label='Address'
                  size="small"
                   multiline
                   rows={3}      
                    variant='outlined'
                  sx={{ mt: 1.5, mb: 5, ml:4,width:'57rem' }}
                />
                 <TextField
                  id='outlined-basic'
                  label='Other Information'
                  size="small"
                   multiline
                   rows={3}      
                    variant='outlined'
                  sx={{ mt: 1.5, mb: 5, ml:4,width:'57rem' }}
                />
              <TextField
                      required
                      size='small'
                      sx={{ marginBottom: '20px',width:'57rem',ml:4 }}
                      id='date'
                      label='First Working Date*'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />
                     <TextField
                      required
                      size='small'
                      sx={{ marginBottom: '20px',width:'57rem',ml:4 }}
                      id='date'
                      label='Last Working Date*'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />
                    <TextField
                  id='outlined-basic'
                  label='First Working Comments'
                  size="small"
                   multiline
                   rows={3}      
                    variant='outlined'
                  sx={{ mt: 1.5, mb: 5, ml:4,width:'57rem' }}
                />
                <TextField
                  id='outlined-basic'
                  label='Last Working Comments'
                  size="small"
                   multiline
                   rows={3}      
                    variant='outlined'
                  sx={{ mt: 1.5, mb: 5, ml:4,width:'57rem' }}
                />


               <Box sx={{ml:5}}>
                 <Button variant='contained' size='medium'>Save As Draft</Button>
                 <Button variant='contained' size='medium' sx={{ml:2}}>Submit</Button>

                 </Box>
                 </Box>

            </Box>
          </Box>
        </Box>
      </Modal>
    </Box>
  )
}

export default SickLeave
