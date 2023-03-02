import {
  Box,
  Button,
  Drawer,
  FormControl,
  FormControlLabel,
  IconButton,
  InputLabel,
  MenuItem,
  Select,
  Switch,
  Tooltip
} from '@mui/material'
import FolderIcon from '@mui/icons-material/Folder'
import RefreshIcon from '@mui/icons-material/Refresh'
import { Archive, ContentCopy, ContentCut, Delete } from '@mui/icons-material'
import { useState } from 'react'
import AddWorkspace from '../add-workspace/AddWorkspace'

const Header = () => {
  let [view, setView] = useState('documentView')
  let [openDrawer, setOpenDrawer] = useState(false)

  const handleView = event => {
    setView(event.target.value)
  }

  const toggleDrawer = action => {
    setOpenDrawer(action)
  }
  // 'linear-gradient(98deg, #C6A7FE, #9155FD 94%)'
  return (
    //  Header Container
    <Box sx={{ display: 'flex', flexDirection: 'column', marginBottom: 20 }}>
      <Box
        sx={{
          display: 'flex',
          marginBottom: '20px',
          padding: 2,
          gap: 5,
          alignContent: 'flex-start',
          justifyContent: 'space-between',
          flexWrap: 'wrap',
          alignItems: 'center'
        }}
      >
        {/* Buttons Container  */}
        {/* New workspace and Refresh button container  */}
        <Box
          sx={{
            display: 'flex',
            flexDirection: {
              xs: 'row',
              sm: 'column',
              md: 'column',
              lg: 'column',
              xl: 'column'
            }
          }}
        >
          <Box
            sx={{
              display: 'flex',
              gap: 2,
              flexDirection: {
                xs: 'column',
                sm: 'row',
                md: 'row',
                lg: 'row',
                xl: 'row'
              }
            }}
          >
            <Button
              variant='outlined'
              sx={{ color: '#9155FD' }}
              size='small'
              startIcon={<FolderIcon />}
              onClick={() => toggleDrawer(true)}
            >
              New Workspace
            </Button>
            <Drawer anchor='right' open={openDrawer} onClose={() => toggleDrawer(false)}>
              <AddWorkspace toggleDrawer={toggleDrawer} />
            </Drawer>
            <Button variant='outlined' size='small' startIcon={<RefreshIcon />}>
              Refresh
            </Button>
          </Box>
          <Box sx={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap' }}>
            <Tooltip title='Copy'>
              <IconButton>
                <ContentCopy />
              </IconButton>
            </Tooltip>
            <Tooltip title='Cut'>
              <IconButton>
                <ContentCut />
              </IconButton>
            </Tooltip>
            <Tooltip title='Delete'>
              <IconButton>
                <Delete />
              </IconButton>
            </Tooltip>
            <Tooltip title='Archive'>
              <IconButton>
                <Archive />
              </IconButton>
            </Tooltip>
          </Box>
        </Box>

        <Box
          sx={{
            display: 'flex',
            flexDirection: {
              md: 'row',
              xs: 'column',
              sm: 'column',
              lg: 'row',
              xl: 'row'
            },
            gap: 3
          }}
        >
          {/* <Box> */}
          <FormControlLabel
            value='top'
            control={<Switch sx={{ width: 80 }} color='primary' />}
            label='Enable Version'
            labelPlacement='start'
          />
          {/* </Box> */}

          {/* <Box> */}
          <FormControl>
            <InputLabel id='select-label'>Select View Type</InputLabel>
            <Select
              sx={{ width: 200 }}
              labelId='select-label'
              id='simple-select'
              value={view}
              label='view'
              onChange={handleView}
            >
              <MenuItem value={'documentView'}>Document View</MenuItem>
              <MenuItem value={'fileView'}>File View</MenuItem>
              <MenuItem value={'folderView'}>Folder View</MenuItem>
              <MenuItem value={'bookView'}>Book View</MenuItem>
            </Select>
          </FormControl>
          {/* </Box> */}
        </Box>
      </Box>
    </Box>
  )
}

export default Header
