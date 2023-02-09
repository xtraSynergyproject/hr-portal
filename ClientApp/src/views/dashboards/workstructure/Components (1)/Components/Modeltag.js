// ** React Imports
import { useState } from 'react'

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
import Tagtable from'src/pages/dashboards/Project/Components/Tagtable'
import FormatListNumberedRtlIcon from '@mui/icons-material/FormatListNumberedRtl';
import Versiontable from 'src/Pages/dashboards/Project/Components/Versiontable'


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

  const handleChange = (event, newValue) => {
    setValue(newValue)
  }

  return (
    <TabContext value={value}>
      {/* <TabList onChange={handleChange} aria-label='customized tabs example'>
        <Tab value='1' label='close' icon={ <CancelIcon />} />
        <Tab value='2' label='Note No'icon={<FormatListNumberedRtlIcon/>} />
        <Tab value='3' label='Draft'icon={<DraftsIcon/> } />
        <Tab value='4' label='Version No' icon={<FormatListNumberedRtlIcon/>} />
        <Tab value='5' label='Attachment'icon={<FilePresentIcon />} />
        <Tab value='6' label='Tags' icon={  <StyleIcon />} />
        <Tab value='7' label='Log' icon={  <StyleIcon />}/>
      </TabList>
      <TabPanel value='1'>
        <Typography>
          <CancelIcon />
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
          <p>1</p>
          <Modeltag/>
        </Typography>
      </TabPanel>
      <TabPanel value='5'>
        <Typography>
          <p>0</p>
          <FilePresentIcon />
        </Typography>
        </TabPanel>
        <TabPanel value='6'>
          <Typography>
            <StyleIcon />
          </Typography>
        </TabPanel>
        <TabPanel value='7'>
          <Typography>
            <StyleIcon />
          </Typography>
        </TabPanel> */}
        {/* <Tagtable/> */}
        <Versiontable/>
      
    </TabContext>
  )
}

export default TabsCustomized
