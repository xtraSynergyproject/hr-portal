import React from 'react'
import { Box, Button, Modal, Typography, Divider } from '@mui/material'
import Icon from 'src/@core/components/icon'

// Icons
import AddCommentIcon from '@mui/icons-material/AddComment'
import RateReviewIcon from '@mui/icons-material/RateReview'
import EmailIcon from '@mui/icons-material/Email'
import NoteVersionModal from './components/NoteVersionModal'
import NoteLogModal from './components/NoteLogModal'
import NoteTagsModal from './components/NoteTagsModal'
import NoteNotificationModal from './components/NoteNotificationModal'
import NoteShareModal from './components/NoteShareModal'
import NoteAttachmentModal from './components/NoteAttachmentModal'

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
  width: '80rem',
  mb: 3,
  borderRadius: '10px'
}

function NotePopup() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <Box>
      <Button variant='contained' onClick={handleOpen}>
        Note
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
            <Box
              sx={{ mb: '10px', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}
              className='demo-space-x'
            >
              <Typography sx={{ pl: 4 }} variant='h5' component='h3'>
                Manage Note
              </Typography>

              <Box
                sx={{
                  // width: '200px',
                  height: '60px',
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignContent: 'center'
                }}
              >
                {/* <Typography
                  sx={{
                    borderRadius: '50px',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    fontSize: '13px',
                    cursor: 'pointer'
                  }}
                  component='label'
                >
                  <Icon icon='mdi:attachment-plus' fontSize='18px' />
                  Attachment
                  <input type='file' hidden />
                </Typography> */}

                <Typography
                  sx={{
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
            </Box>
            <Divider />

            <Box sx={{ display: 'flex' }}>
              <Box sx={{ flex: 1.5, mx: '5px', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <small>Note No.</small>
                <Typography fontSize='medium' sx={{ color: 'lightslategray' }}>
                  N-120.665445-45
                </Typography>
              </Box>

              <Box sx={{ flex: 1, mx: '5px', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <small>Status</small>
                <Typography fontSize='medium' sx={{ color: 'lightslategray' }}>
                  Active
                </Typography>
              </Box>

              <NoteVersionModal />

              <Box sx={{ flex: 1, mx: '5px', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <small>Priority</small>
                <Typography fontSize='medium' sx={{ color: 'lightslategray' }}>
                  Medium
                </Typography>
              </Box>

              <Box sx={{ flex: 1, mx: '5px', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <small>Start Date</small>
                <Typography fontSize='medium' sx={{ color: 'lightslategray' }}>
                  12-05-22
                </Typography>
              </Box>

              <Box sx={{ flex: 1, mx: '5px', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <small>Expiry Date</small>
                <Typography fontSize='medium' sx={{ color: 'lightslategray' }}>
                  12-05-22
                </Typography>
              </Box>

              <NoteNotificationModal/>

              <Box sx={{ flex: 0.75, mx: '5px', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <small>Comment</small>
                <AddCommentIcon sx={{ color: 'lightslategray' }} fontSize='large' />
              </Box>

             <NoteShareModal/>

              <Box sx={{ flex: 0.75, mx: '5px', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <small>Email</small>
                <EmailIcon sx={{ color: 'lightslategray' }} fontSize='large' />
              </Box>

              <Box sx={{ flex: 0.75, mx: '5px', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <small>Inline_Comment</small>
                <RateReviewIcon sx={{ color: 'lightslategray' }} fontSize='large' />
              </Box>

              <NoteAttachmentModal/>

              <NoteTagsModal/>

              <NoteLogModal/>
            </Box>
          </Box>
        </Box>
      </Modal>
    </Box>
  )
}

export default NotePopup
