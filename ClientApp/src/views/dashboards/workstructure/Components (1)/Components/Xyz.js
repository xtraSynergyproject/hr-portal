// ** React Imports
import React, { useState } from 'react'


// ** MUI Imports
import Tab from '@mui/material/Tab'
import TabPanel from '@mui/lab/TabPanel'
import TabContext from '@mui/lab/TabContext'
import { styled } from '@mui/material/styles'
import Typography from '@mui/material/Typography'
import MuiTabList from '@mui/lab/TabList'
import CancelIcon from '@mui/icons-material/Cancel'
import FilePresentIcon from '@mui/icons-material/FilePresent'
import StyleIcon from '@mui/icons-material/Style'
import DraftsIcon from '@mui/icons-material/Drafts';
import Icon from 'src/@core/components/icon'
import Versionmodule from './Versionmodule'
import Tmodule from'./Tmodule'
import Logmodule from'./Logmodule'
import Attachmentmodule from'./Attachmentmodule'

import FormatListNumberedRtlIcon from '@mui/icons-material/FormatListNumberedRtl';


// Styled TabList component
const TabList = styled(MuiTabList)(({ theme }) => ({
  '& .MuiTabs-indicator': {
    display: 'none'
  },
  '& .Mui-selected': {
    backgroundColor: theme.palette.primary.main,
    color: `${theme.palette.common.white} !important`
  },
  '& .MuiTab-root': {
    minHeight: 38,
    minWidth: 130,
    borderRadius: theme.shape.borderRadius
  }
}))

const TabsCustomized = () => {
  // ** State
  const [value, setValue] = useState('1')
  
  const [open, setOpen] = useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const handleChange = (event, newValue) => {
    setValue(newValue)
  }

  return (
    <TabContext value={value}>
      <TabList onChange={handleChange} aria-label='customized tabs example'>
      {/* <Tab value='1' label='close' icon={ <CancelIcon />} onClick={handleClose}/> */}
        <Tab value='2' label='Note No'icon={<FormatListNumberedRtlIcon/>} sx={{ml:2.5}} />
        <Tab value='3' label='Draft'icon={<DraftsIcon/> } />
        <Tab value='4' label='Version No' icon={<FormatListNumberedRtlIcon/>} />
        <Tab value='5' label='Attachment'icon={<FilePresentIcon />} />
        <Tab value='6' label='Tags' icon={  <StyleIcon />} />
        <Tab value='7' label='Log' icon={  <StyleIcon />}/>
      </TabList>
      <TabPanel value='1'>
        <Typography>
          {/* <CancelIcon /> */}
        </Typography>
      </TabPanel>
      <TabPanel value='2'>
        <Typography>
          <p>N-26-12-2022-57</p>
        </Typography>
      </TabPanel>
      <TabPanel value='3'>
        <Typography>
          <p>Staus</p>
          </Typography>
      </TabPanel>
      <TabPanel value='4'>
        <Typography>
          <Versionmodule/>
        </Typography>
      </TabPanel>
      <TabPanel value='5'>
        <Typography>
          <Attachmentmodule/>
          
          {/* <FilePresentIcon /> */}
        </Typography>
        </TabPanel>
        <TabPanel value='6'>
          <Typography>
          <Tmodule/>
            {/* <StyleIcon /> */}
          </Typography>
        </TabPanel>
        <TabPanel value='7'>
          <Typography>
          <Logmodule/>
          </Typography>
        </TabPanel>
      
    </TabContext>
  )
}

export default TabsCustomized
