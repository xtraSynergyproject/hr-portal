import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
import MenuBtn from './MenuBtn'
import { FormControl, InputLabel, Select, MenuItem } from '@mui/material'

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

function AddTransferReqModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  const [value, setValue] = React.useState('')
  const handleChanges = event => {
    setValue(event.target.value)
  }

  return (
    <Box>
      <Button variant='contained' onClick={handleOpen}>
        Termination Request
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
              <Typography sx={{ p: 4 }} variant='h4' component='h3'>
                Termination Request
              </Typography>

              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignContent: 'center' }}>
                <Button sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }} component='label'>
                  <Icon icon='mdi:attachment-plus' />
                  Attachment
                  <input type='file' hidden />
                </Button>
                <MenuBtn />

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
            <hr />

            <Box sx={{ mx: 4 }}>
              <Box>
                <Box sx={{ p: 3 }}>
                  <Typography sx={{ my: 3 }}>
                    Service Owner/Requester: <b>{}</b>
                  </Typography>
                  <Typography sx={{ my: 3 }}>
                    Service Number: <b>{}</b>
                  </Typography>
                  <Typography sx={{ my: 3 }}>
                    Service Status: <b>{}</b>
                  </Typography>
                  <Typography sx={{ my: 3 }}>
                    Due Date: <b>{}</b>
                  </Typography>
                  <Typography sx={{ my: 3 }}>
                    Service Version: <b>{}</b>
                  </Typography>
                </Box>{' '}
              </Box>

              <Box sx={{display:"flex", justifyContent:"space-between"}}>
                <FormControl sx={{ width: '580px' }}>
                  <InputLabel required id='demo-simple-select-label'>
                    Location
                  </InputLabel>
                  <Select
                    labelId='demo-simple-select-label'
                    id='demo-simple-select'
                    value={value}
                    label='Location'
                    onChange={handleChanges}
                  >
                    <MenuItem value='Abu Dhabi'>Abu Dhabi</MenuItem>
                    <MenuItem value='Sharjah'>Sharjah</MenuItem>
                    <MenuItem value='Al Mankhool'>Al Mankhool</MenuItem>
                    <MenuItem value='Satwa'>Satwa</MenuItem>
                    <MenuItem value='Kuwait'>Kuwait</MenuItem>
                    <MenuItem value='Saudi Arabia'>Saudi Arabia</MenuItem>
                    <MenuItem value='Lebanon'>Lebanon</MenuItem>
                    <MenuItem value='Jordan'>Jordan</MenuItem>
                    <MenuItem value='Egypt'>Egypt</MenuItem>
                    <MenuItem value='Bahrain'>Bahrain</MenuItem>
                    <MenuItem value='UAE'>UAE</MenuItem>
                    <MenuItem value='Tunisia'>Tunisia</MenuItem>
                    <MenuItem value='Morocco'>Morocco</MenuItem>
                    <MenuItem value='Bhopal'>Bhopal</MenuItem>
                  </Select>
                </FormControl>

                <Box sx={{ display: 'flex', my: 5, justifyContent: 'flex-end', gap: 3 }}>
                  <Button variant='contained'>Save As Draft</Button>
                  <Button variant='contained'>Submit</Button>
                </Box>
              </Box>
            </Box>
          </Box>
        </Box>
      </Modal>
    </Box>
  )
}

export default AddTransferReqModal
