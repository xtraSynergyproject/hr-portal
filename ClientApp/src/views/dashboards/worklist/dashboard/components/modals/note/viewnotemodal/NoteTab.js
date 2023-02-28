// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Tab from '@mui/material/Tab'
import TabList from '@mui/lab/TabList'
import TabPanel from '@mui/lab/TabPanel'
import TabContext from '@mui/lab/TabContext'
import PersonDocuments from '../../servicemodal/PersonDocuments';
import CoreHr1 from '../../note/CoreHr1';



const TabsVertical = () => {
    // ** State
    const [value, setValue] = useState('1')

    const handleChange = (event, newValue) => {
        setValue(newValue)
    }

    return (
        <TabContext value={value}>
        <Box sx={{ display: 'flex' }}>
            <TabList orientation='vertical' onChange={handleChange} aria-label='vertical tabs example' sx={{textTransform:'capitalize',textAlign:'center'}}>
              

                <Tab value='1' label='PersonDocuments' sx={{textTransform:'capitalize', backgroundColor:'#FFF8DC',ml:9}} />
                <Tab value='2' label='CoreHR' sx={{textTransform:'capitalize', backgroundColor:'#FFF8DC',ml:9,mt:2}}/>
                <Tab value='3' label='Help Center' sx={{textTransform:'capitalize', backgroundColor:'#FFF8DC',ml:9,mt:2}} />


            </TabList>
            
            
            <TabPanel value='1'>
                <PersonDocuments />
            </TabPanel>
            <TabPanel value='2'>
            <CoreHr1 />

            </TabPanel>
            <TabPanel value='3'>
            

            </TabPanel>
            
        </Box>
    </TabContext>









            )
}

export default TabsVertical
