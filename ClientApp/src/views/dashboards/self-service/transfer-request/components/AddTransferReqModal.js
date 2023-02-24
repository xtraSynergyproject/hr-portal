import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
import MenuBtn from 'src/views/dashboards/hr-only/termination/components/button_with_modal/MenuBtn'
import { FormControl, InputLabel, Select, MenuItem, Divider } from '@mui/material'
import ModalUserInfo from 'src/views/dashboards/hr-only/termination/ModalUserInfo'

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
        Add Transfer Request
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
            <Box sx={{ mx: 4 }}>
              <Box
                sx={{ mb: '10px', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}
                className='demo-space-x'
              >
                <Typography sx={{ pl: 4 }} variant='h5' component='h3'>
                  Add Misconduct
                </Typography>

                <Box
                  sx={{
                    width: '200px',
                    height: '60px',
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignContent: 'center'
                  }}
                >
                  <Typography
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
                  </Typography>
                  <MenuBtn />

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
              <ModalUserInfo />
              <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
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
