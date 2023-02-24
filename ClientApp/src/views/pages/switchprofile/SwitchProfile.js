import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
import { Divider } from '@mui/material'
import Select from '@mui/material/Select'
import FormControl from '@mui/material/FormControl'
import InputLabel from '@mui/material/InputLabel'
import MenuItem from '@mui/material/MenuItem'

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
  mt: 10,
  width: '60rem',
  mb: 3,
  borderRadius: '10px'
}

function TerminationRequestModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <Box>
      <Typography onClick={handleOpen}>Switch Profile </Typography>

      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby='modal-modal-title'
        aria-describedby='modal-modal-description'
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>
            <Box sx={{ mb: '2px', display: 'flex', justifyContent: 'space-between' }} className='demo-space-x'>
              <Typography sx={{ pl: 6 }}>
                <b> Switch Profile</b>
              </Typography>

              <Box
                sx={{
                  width: '200px',
                  height: '40px',
                  display: 'flex',
                  justifyContent: 'flex-end',
                  alignContent: 'center',
                  float: 'right'
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
                  onClick={handleClose}
                  component='label'
                >
                  <Icon icon='mdi:close' fontSize='24px' />
                </Typography>
              </Box>
            </Box>
            <Divider />
            <br />

            <FormControl sx={{ ml: '26px' }} size='small'>
              <InputLabel id='demo-select-small'>Switch User</InputLabel>
              <Select sx={{ width: '900px', height: '50px' }} labelId='demo-select-small' id='demo-select-small'>
                <MenuItem value={10}> HR Manager</MenuItem>
                <MenuItem value={20}>HR Support</MenuItem>
              </Select>
            </FormControl>
           <br/>
            <br />
            <Divider />
            <Box sx={{ m: 4 }}>
              <Box sx={{ display: 'flex', my: 5, justifyContent: 'flex-end', gap: 3 }}>
                <Button href='#' variant='contained'>
                  Switch
                </Button>
              </Box>
            </Box>
          </Box>
        </Box>
      </Modal>
    </Box>
  )
}

export default TerminationRequestModal
