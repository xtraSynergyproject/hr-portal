import React, { useState } from 'react'
import Box from '@mui/material/Box'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import AttachmentIcon from '@mui/icons-material/Attachment'
import Icon from 'src/@core/components/icon'
import NoteAudioAttachment from './attachment-components/NoteAudioAttachment'
import { Card } from '@mui/material'
import Tab from '@mui/material/Tab'
import TabContext from '@mui/lab/TabContext'
import Tabs from '@mui/material/Tabs'
import TabPanel from '@mui/lab/TabPanel'
import SettingsVoiceIcon from '@mui/icons-material/SettingsVoice'
import UploadFileIcon from '@mui/icons-material/UploadFile'
import CameraIcon from '@mui/icons-material/Camera'
import VideoCameraFrontIcon from '@mui/icons-material/VideoCameraFront'
import NoteAttFileUpload from './attachment-components/NoteAttFIleUpload'
import FileUploaderMultiple from './attachment-components/NoteAttFIleUpload'
import { styled } from '@mui/material/styles'
import DeleteIcon from '@mui/icons-material/Delete'
import VisibilityIcon from '@mui/icons-material/Visibility';
import DownloadIcon from '@mui/icons-material/Download';


const modalWrapper = {
  overflow: 'scroll',
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
  minHeight:"60vh",
  mb: 3,
  borderRadius: '10px'
}


const ProfilePicture = styled('img')(({ theme }) => ({
  width: 120,
  height: 120,
  borderRadius: theme.shape.borderRadius,
  border: `5px solid ${theme.palette.common.white}`,
  [theme.breakpoints.down('md')]: {
    marginBottom: theme.spacing(4)
  }
}))

export default function NoteAttachmentModal() {
  const [open, setOpen] = useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  const [value, setValue] = React.useState('1')

  const handleChange = (event, newValue) => {
    setValue(newValue)
  }
  return (
    <div>
      <Box
        onClick={handleOpen}
        sx={{
          flex: 0.75,
          mx: '5px',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          cursor: 'pointer'
        }}
      >
        <small>Attachment</small>
        <AttachmentIcon sx={{ color: 'lightslategray' }} fontSize='large' />
      </Box>
      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby='modal-modal-title'
        aria-describedby='modal-modal-description'
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>
            <Box
              sx={{ display: 'flex', justifyContent: 'space-between', alignContent: 'center', alignItems: 'center' }}
            >
              <Typography sx={{ p: 5 }} variant='h5' component='h3'>
                Notifications
              </Typography>

              <Typography
                sx={{
                  mx: '10px',
                  borderRadius: '50px',
                  display: 'flex',
                  flexDirection: 'column',
                  alignItems: 'center',
                  fontSize: '13px',
                  cursor: 'pointer'
                }}
                onClick={handleClose}
                component='label'
              >
                <Icon icon='mdi:close' fontSize='18px' />
                Close
              </Typography>
            </Box>
            <hr />

            <Box sx={{ width: '100%', typography: 'body1' }}>
              <TabContext value={value}>
                <Box sx={{ borderBottom: 1, borderColor: 'divider', display: 'flex', justifyContent: 'center' }}>
                  <Tabs value={value} onChange={handleChange} aria-label='icon tabs example'>
                    <Tab icon={<UploadFileIcon />} value='1' label='Click to upload File' />
                    <Tab icon={<SettingsVoiceIcon />} value='2' label='Click for audio recording' />
                    <Tab icon={<CameraIcon />} value='3' label='Click for screen recording' />
                    <Tab icon={<VideoCameraFrontIcon />} value='4' label='Click for video recording' />
                  </Tabs>
                </Box>
                <TabPanel value='1'>
                  <Box sx={{border:"2px dashed #D3D3D3"}}>
                    
                  <FileUploaderMultiple/>
                  </Box>
                </TabPanel>
                <TabPanel value='2'>
                  <NoteAudioAttachment />
                </TabPanel>
                <TabPanel value='3'>Item four</TabPanel>
                <TabPanel value='4'>Item Three</TabPanel>
              </TabContext>

<Typography variant='h6' component="h6" sx={{mx:"15px"}}>Attachment List</Typography>
              <Box
                sx={{
                  height: '90px',
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  p: '10px',
                  m: '10px',
                  border: '1px #D3D3D3 solid',
                  borderRadius: '7px'
                }}
              >
                <img
                  //   src={data.PhotoName}
                  src={''}
                  alt='doc'
                  sx={{ width: '70px', height: '70px', border: '5px solid #f0f0f0' }}
                />

                <Typography sx={{display:"flex",flexDirection:"column", alignItems:"center"}}> <small>Document Name:</small>  <big> pdf{}</big></Typography>
                <Typography sx={{display:"flex",flexDirection:"column", alignItems:"center"}}> <small> Size:</small>  <big>5kb {}</big> </Typography>
                <Typography sx={{display:"flex", alignItems:"center"}}>
                  <VisibilityIcon sx={{cursor:"pointer", mx:"8px"}}/>
                  <DownloadIcon sx={{cursor:"pointer", mx:"8px"}}/>
                  <DeleteIcon sx={{cursor:"pointer", mx:"8px"}} />
                </Typography>
              </Box>

            </Box>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
